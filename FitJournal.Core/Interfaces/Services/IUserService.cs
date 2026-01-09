using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Dtos.Responses.Users;

namespace FitJournal.Core.Interfaces.Services;

public interface IUserService : IBusinessService
{
    Task<UsersResponse> GetAllAsync(UserPaginationRequest request, CancellationToken token);

    Task<UserResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task EditAsync(EditUserRequest request, Guid id, CancellationToken token);
}
