using FitnessTracker.App.Dtos.Requests.Users;
using FitnessTracker.App.Dtos.Responses.Users;

namespace FitnessTracker.App.Services.Interfaces;

public interface IUserService
{
    Task<GetProfileResponse> GetProfileAsync(Guid id, CancellationToken token = default);
    Task UpdateProfileAsync(UpdateProfileRequest request, Guid id, CancellationToken token = default);
}
