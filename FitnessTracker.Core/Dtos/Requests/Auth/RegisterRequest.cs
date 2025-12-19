using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Requests.Auth;

public record RegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmedPassword { get; init; }
    public required string Phone { get; init; }
    public DateOnly? Birthday { get; init; }
    public double? Height { get; init; }
    public double? Weight { get; init; }
    public Gender? Gender { get; init; }
}
