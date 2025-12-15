using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Dtos.Responses.Auth;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FitnessTracker.Infra.Config;

namespace FitnessTracker.App.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthService
{
    private static readonly JwtSecurityTokenHandler _tokenHandler = new();
    private static readonly SymmetricSecurityKey _secret = new(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret));

    public async Task RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        if (request.Birthday is DateOnly birthday && DateOnly.FromDateTime(DateTime.UtcNow) < birthday.AddYears(13))
            throw new BadRequestException(ErrorMessages.AgeRestriction);

        var user = mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await unitOfWork.UserRepository.AddAsync(user, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByEmailAsync(request.Email, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserEmailNotFound, request.Email));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException(ErrorMessages.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    private string GenerateToken(User user, TokenType type)
        => _tokenHandler.WriteToken(_tokenHandler.CreateToken(new()
        {
            Issuer = AppConfig.Auth.Issuer,
            Audience = AppConfig.Auth.Audience,
            Subject = new(
                [
                    new("userId", $"{user.Id}"),
                    new("role", $"{user.Role}")
                ]),
            SigningCredentials = new(_secret, SecurityAlgorithms.HmacSha256),
            Expires = type == TokenType.Access
                ? DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes)
                : DateTime.UtcNow.AddDays(AppConfig.Auth.RefreshTokenLifetimeDays)
        }));
}
