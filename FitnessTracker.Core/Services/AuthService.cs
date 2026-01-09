using AutoMapper;
using FitnessTracker.Core.Config;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FitnessTracker.Core.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper, IUserValidator userValidator)
    : BusinessService(unitOfWork, mapper), IAuthService
{
    private readonly IUserValidator _userValidator = userValidator;

    private static readonly JwtSecurityTokenHandler _tokenHandler = new();
    private static readonly SymmetricSecurityKey _secret = new(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret));

    public async Task RegisterAsync(RegisterRequest request, CancellationToken token)
    {
        await _userValidator.ValidateRegisterAsync(request, token);

        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _unitOfWork.Users.AddAsync(user, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, token)
            ?? throw new NotFoundException(BusinessErrors.Users.EmailNotFound(request.Email));

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BadRequestException(BusinessErrors.Users.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        await _unitOfWork.Users.RemoveAsync(user, hardDelete: false, token);
        await _unitOfWork.CommitAsync(token);
    }

    private static string GenerateToken(User user, TokenType type) =>
        _tokenHandler.WriteToken(_tokenHandler.CreateToken(new()
        {
            Issuer = AppConfig.Auth.Issuer,
            Audience = AppConfig.Auth.Audience,
            Subject = new([
                new("userId", $"{user.Id}"),
                new("role", $"{user.Role}")]),
            SigningCredentials = new(_secret, SecurityAlgorithms.HmacSha256),
            Expires = type == TokenType.Access
                ? DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes)
                : DateTime.UtcNow.AddDays(AppConfig.Auth.RefreshTokenLifetimeDays)
        }));
}
