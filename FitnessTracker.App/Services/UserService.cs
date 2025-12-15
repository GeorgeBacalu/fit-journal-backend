using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Users;
using FitnessTracker.App.Dtos.Responses.Users;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.App.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<GetProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, id));

        return mapper.Map<GetProfileResponse>(user);
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest request, Guid id, CancellationToken token = default)
    {
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
