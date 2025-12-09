using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.App.Services;
public class AuthService(IUnitOfWork unitOfWork, IMapper mapper) : IAuthService
{
    public async Task RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        if (DateTime.UtcNow.Year - request.Birthday.Year < 13)
            throw new BadRequestException(ErrorMessageConstants.AgeRestriction);

        var user = mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await unitOfWork.UserRepository.AddAsync(user, token);
        await unitOfWork.CommitAsync(token);
    }
}
