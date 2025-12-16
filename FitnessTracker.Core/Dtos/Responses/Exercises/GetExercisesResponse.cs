namespace FitnessTracker.Core.Dtos.Responses.Exercises;

public class GetExercisesResponse
{
    public IEnumerable<GetExerciseResponse> Exercises { get; init; } = [];
}
