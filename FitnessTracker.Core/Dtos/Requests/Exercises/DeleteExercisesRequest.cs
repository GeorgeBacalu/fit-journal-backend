namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public class DeleteExercisesRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
}
