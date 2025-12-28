using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IUserService : IBusinessService
{
    Task<UsersResponse> GetAllAsync(CancellationToken token);

    Task<UserResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task EditAsync(EditUserRequest request, Guid id, CancellationToken token);
}
