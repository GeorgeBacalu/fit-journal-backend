namespace FitnessTracker.Core.Dtos.Responses.Auth;

public record LoginResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
