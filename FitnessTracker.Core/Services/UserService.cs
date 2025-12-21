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
    public async Task<UsersResponse> GetAllAsync(CancellationToken token)
    {
        var users = await unitOfWork.Users.GetAllAsync(token);

        return new()
        {
            Users = mapper.Map<IEnumerable<ShortUserResponse>>(users),
            TotalCount = users.Count()
        };
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, id));

        return mapper.Map<UserResponse>(user);
    }

    public async Task EditAsync(EditUserRequest request, Guid id, CancellationToken token)
    {
        if (await unitOfWork.Users.AnyAsync(user => user.Name == request.Name && user.Id != id, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await unitOfWork.Users.AnyAsync(user => user.Email == request.Email && user.Id != id, token))
            throw new BadRequestException(ValidationErrors.Users.EmailTaken);

        var user = await unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, id));

        mapper.Map(request, user);

        await unitOfWork.CommitAsync(token);
    }
}
