namespace FitnessTracker.Core.Dtos.Responses.Users;

public record UserResponse : ShortUserResponse
{
    public required string Email { get; init; }
    public required string Phone { get; init; }

    public int Age => DateTime.UtcNow.Year - Birthday.Year - (DateTime.UtcNow.DayOfYear < Birthday.DayOfYear ? 1 : 0);
}
