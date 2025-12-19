using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Users;

public record ProfileResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public DateOnly Birthday { get; init; }
    public double Height { get; init; }
    public double Weight { get; init; }
    public Gender Gender { get; init; }
    
    public int Age => DateTime.UtcNow.Year - Birthday.Year - (DateTime.UtcNow.DayOfYear < Birthday.DayOfYear ? 1 : 0);
}
