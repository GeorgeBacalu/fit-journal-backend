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
    private static readonly JwtSecurityTokenHandler _tokenHandler = new();
    private static readonly SymmetricSecurityKey _secret = new(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret));

    public async Task RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        var user = mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await unitOfWork.UserRepository.AddAsync(user, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetAsync(user => user.Email == request.Email, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserEmailNotFound, request.Email));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException(ErrorMessages.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    public async Task DeleteAccountAsync(Guid? id, Guid userId, CancellationToken token = default)
    {
        var currentUser = await unitOfWork.UserRepository.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, userId));

        var targetUserId = id ?? userId;

        if (currentUser.Role != Role.Admin && targetUserId != userId)
            throw new ForbiddenException(ErrorMessages.UnauthorizedAccountDeletion);

        var targetUser = await unitOfWork.UserRepository.GetByIdAsync(targetUserId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, targetUserId));

        await unitOfWork.UserRepository.RemoveAsync(targetUser, false, token);
        await unitOfWork.CommitAsync(token);
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
