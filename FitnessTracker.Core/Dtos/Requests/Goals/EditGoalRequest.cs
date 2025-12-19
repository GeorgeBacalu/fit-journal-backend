namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record EditGoalRequest : AddGoalRequest
{
    public Guid Id { get; init; }
}
