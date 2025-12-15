namespace FitnessTracker.App.Dtos.Responses.Auth;

public class LoginResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
