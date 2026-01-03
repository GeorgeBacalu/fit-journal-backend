using FitnessTracker.Core.Dtos.Requests.Users;

namespace FitnessTracker.Core.Interfaces.Services.Admin;

public interface IAdminUserService : IBusinessService
{
    Task EditAsync(EditUserRequest request, Guid id, CancellationToken token);
}
