namespace FitnessTracker.Core.Dtos.Responses.Users;

public record UsersResponse
{
    public IEnumerable<ShortUserResponse> Users { get; init; } = [];
}
