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
    public async Task<UsersResponse> GetAllAsync(CancellationToken token = default)
    {
        var users = await unitOfWork.Users.GetAllAsync(token);

        return new()
        {
            Users = mapper.Map<IEnumerable<ShortUserResponse>>(users),
            TotalCount = users.Count()
        };
    }

    public async Task<ProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, id));

        return mapper.Map<ProfileResponse>(user);
    }

    public async Task EditProfileAsync(EditProfileRequest request, Guid id, CancellationToken token = default)
    {
        if (await unitOfWork.Users.AnyAsync(user => user.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        if (await unitOfWork.Users.AnyAsync(user => user.Email == request.Email, token))
            throw new BadRequestException(ValidationErrors.EmailTaken);

        var user = await unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, id));

        mapper.Map(request, user);

        await unitOfWork.CommitAsync(token);
    }
}
