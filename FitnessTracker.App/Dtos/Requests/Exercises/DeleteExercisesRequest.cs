namespace FitnessTracker.App.Dtos.Requests.Exercises;

public class DeleteExercisesRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
}
