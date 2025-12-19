using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IUserService
{
    Task<UsersResponse> GetAllAsync(CancellationToken token = default);
    Task<ProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default);
    Task EditProfileAsync(EditProfileRequest request, Guid id, CancellationToken token = default);
}
