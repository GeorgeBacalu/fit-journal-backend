namespace FitJournal.Core.Dtos.Responses.Exercises;

public record ExercisesResponse
{
    public IEnumerable<ShortExerciseResponse> Exercises { get; init; } = [];
    public int TotalCount { get; init; }
}
