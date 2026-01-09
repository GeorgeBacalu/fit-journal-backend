namespace FitJournal.Core.Dtos.Responses.Exercises;

public record ExerciseResponse : ShortExerciseResponse
{
    public string? Description { get; init; }
    public string? Notes { get; init; }
}
