using AutoMapper;
using FitJournal.Core.Config;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Results;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.Auth;
using FitJournal.Domain.Enums.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FitJournal.Core.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IUserValidator userValidator)
    : BusinessService(unitOfWork, mapper), IAuthService
{
    private readonly IEmailService _emailService = emailService;
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
            throw new BadRequestException(BusinessErrors.Auth.InvalidCredentials);

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    public async Task<LoginResponse> ExternalLoginAsync(string email, string name, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetAsync(u => u.Email == email, token);
        if (user == null)
        {
            user = new User
            {
                Name = string.IsNullOrWhiteSpace(name) ? email.Split('@')[0] : name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N")),
                Phone = string.Empty,
                Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18)),
                Height = 170,
                Weight = 70,
                Gender = Gender.Unknown,
                Role = Role.User
            };
            await _unitOfWork.Users.AddAsync(user, token);
            await _unitOfWork.CommitAsync(token);
        }

        return new() { AccessToken = GenerateToken(user, TokenType.Access), RefreshToken = GenerateToken(user, TokenType.Refresh) };
    }

    public async Task<RefreshResponse> RefreshAsync(RefreshRequest request, CancellationToken token)
    {
        if (!Guid.TryParse(GetUser(request.RefreshToken, TokenType.Refresh, BusinessErrors.Auth.InvalidRefreshToken).FindFirstValue("userId"), out var id))
            throw new UnauthorizedException(BusinessErrors.Auth.NoRefreshTokenUserInfo);

        var user = await _unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        return new()
        {
            AccessToken = GenerateToken(user, TokenType.Access),
            RefreshToken = GenerateToken(user, TokenType.Refresh)
        };
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request, Guid id, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new BadRequestException(BusinessErrors.Auth.WrongCurrentPassword);

        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            throw new BadRequestException(BusinessErrors.Auth.SamePassword);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, token)
            ?? throw new NotFoundException(BusinessErrors.Users.EmailNotFound(request.Email));

        var resetToken = new ResetToken
        {
            UserId = user.Id,
            Token = GenerateToken(user, TokenType.Reset),
            ExpiresAt = DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes),
        };
        await _unitOfWork.ResetTokens.AddAsync(resetToken, token);
        await _unitOfWork.CommitAsync(token);

        await _emailService.SendAsync(new()
        {
            To = user.Email,
            Subject = "Reset Your Password",
            Body = _emailService.GeneratePasswordResetEmail(new()
            {
                UserName = user.Name,
                ResetLink = $"{AppConfig.ExternalAuth.FrontendUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(resetToken.Token)}",
                ExpiresAt = resetToken.ExpiresAt
            })
        }, token);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken token)
    {
        if (!Guid.TryParse(GetUser(request.Token, TokenType.Reset, BusinessErrors.Auth.InvalidResetToken).FindFirstValue("userId"), out var id))
            throw new UnauthorizedException(BusinessErrors.Auth.NoResetTokenUserInfo);

        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        var resetToken = await _unitOfWork.ResetTokens.GetLastAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Auth.ResetTokenNotFound);

        if (resetToken.ExpiresAt <= DateTime.UtcNow || resetToken.Used || !TokensMatch(request.Token, resetToken.Token))
            throw new BadRequestException(BusinessErrors.Auth.ResetTokenSpent);

        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            throw new BadRequestException(BusinessErrors.Auth.SamePassword);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        resetToken.Used = true;
        await _unitOfWork.CommitAsync(token);
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
                new("role", $"{user.Role}"),
                new("token_type", type.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))]),
            SigningCredentials = new(_secret, SecurityAlgorithms.HmacSha256),
            Expires = type switch
            {
                TokenType.Refresh => DateTime.UtcNow.AddDays(AppConfig.Auth.RefreshTokenLifetimeDays),
                _ => DateTime.UtcNow.AddMinutes(AppConfig.Auth.AccessTokenLifetimeMinutes)
            }
        }));

    private static ClaimsPrincipal GetUser(string token, TokenType expectedType, Error error)
    {
        try
        {
            var principal = _tokenHandler.ValidateToken(token, new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppConfig.Auth.Issuer,
                ValidAudience = AppConfig.Auth.Audience,
                IssuerSigningKey = _secret
            }, out var jwt);

            return jwt is JwtSecurityToken { Header.Alg: SecurityAlgorithms.HmacSha256 }
                && principal.FindFirstValue("token_type") == expectedType.ToString()
                ? principal
                : throw new UnauthorizedException(error);
        }
        catch
        {
            throw new UnauthorizedException(error);
        }
    }

    private static bool TokensMatch(string submitted, string stored) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(submitted),
            Encoding.UTF8.GetBytes(stored));
}
