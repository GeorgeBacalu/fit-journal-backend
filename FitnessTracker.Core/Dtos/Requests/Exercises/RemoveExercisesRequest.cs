namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record RemoveExercisesRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}
