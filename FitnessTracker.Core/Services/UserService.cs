using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<ProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, id));

        return mapper.Map<ProfileResponse>(user);
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest request, Guid id, CancellationToken token = default)
    {
        if (request.Birthday is DateOnly birthday && DateOnly.FromDateTime(DateTime.UtcNow) < birthday.AddYears(13))
            throw new BadRequestException(ErrorMessages.AgeRestriction);

        var user = await unitOfWork.UserRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, id));

        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        user.Phone = request.Phone ?? user.Phone;
        user.Birthday = request.Birthday ?? user.Birthday;
        user.Height = request.Height ?? user.Height;
        user.Weight = request.Weight ?? user.Weight;
        user.Gender = request.Gender ?? user.Gender;

        await unitOfWork.CommitAsync(token);
    }
}
