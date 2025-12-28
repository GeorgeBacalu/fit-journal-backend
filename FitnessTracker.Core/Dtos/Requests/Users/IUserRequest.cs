namespace FitnessTracker.Core.Dtos.Requests.Users;

public interface IUserRequest
{
    string Name { get; }
    string Email { get; }
}
