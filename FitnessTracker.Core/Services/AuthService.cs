using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FitnessTracker.Core.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthService
{
    private static readonly JwtSecurityTokenHandler tokenHandler = new();
    private static readonly SymmetricSecurityKey secret = new(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret));

    public async Task RegisterAsync(RegisterRequest request, CancellationToken token)
    {
        if (await unitOfWork.Users.AnyAsync(user => user.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await unitOfWork.Users.AnyAsync(user => user.Email == request.Email, token))
            throw new BadRequestException(ValidationErrors.Users.EmailTaken);

        var user = mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await unitOfWork.Users.AddAsync(user, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetAsync(user => user.Email == request.Email, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.EmailNotFound, request.Email));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException(ErrorMessages.Users.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, id));

        await unitOfWork.Users.RemoveAsync(user, hardDelete: false, token);
        await unitOfWork.CommitAsync(token);
    }

    private static string GenerateToken(User user, TokenType type) =>
        tokenHandler.WriteToken(tokenHandler.CreateToken(new()
        {
            Issuer = AppConfig.Auth.Issuer,
            Audience = AppConfig.Auth.Audience,
            Subject = new(
                [
                    new("userId", $"{user.Id}"),
                    new("role", $"{user.Role}")
                ]),
            SigningCredentials = new(secret, SecurityAlgorithms.HmacSha256),
            Expires = type == TokenType.Access
                ? DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes)
                : DateTime.UtcNow.AddDays(AppConfig.Auth.RefreshTokenLifetimeDays)
        }));
}
