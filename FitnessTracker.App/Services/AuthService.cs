using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Dtos.Responses;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FitnessTracker.App.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthService
{
    private static readonly JwtSecurityTokenHandler tokenHandler = new();
    private static readonly SymmetricSecurityKey secret = new(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret));

    public async Task RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        if (request.Birthday is DateOnly birthday && DateOnly.FromDateTime(DateTime.UtcNow) < birthday.AddYears(13))
            throw new BadRequestException(ErrorMessageConstants.AgeRestriction);

        var user = mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await unitOfWork.UserRepository.AddAsync(user, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException(string.Format(ErrorMessageConstants.UserEmailNotFound, request.Email));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException(ErrorMessageConstants.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user),
            RefreshToken = GenerateToken(user, isAccessToken: false)
        };
    }

    private string GenerateToken(User user, bool isAccessToken = true)
        => tokenHandler.WriteToken(tokenHandler.CreateToken(new()
        {
            Issuer = AppConfig.Auth.Issuer,
            Audience = AppConfig.Auth.Audience,
            Subject = new(
                [
                    new("userId", $"{user.Id}"),
                    new("role", $"{user.Role}")
                ]),
            SigningCredentials = new(secret, SecurityAlgorithms.HmacSha256),
            Expires = isAccessToken
                ? DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes)
                : DateTime.UtcNow.AddDays(AppConfig.Auth.RefreshTokenLifetimeDays)
        }));
}
