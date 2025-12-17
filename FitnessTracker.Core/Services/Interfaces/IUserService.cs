using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IUserService
{
    Task<ProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default);
    Task UpdateProfileAsync(UpdateProfileRequest request, Guid id, CancellationToken token = default);
}
