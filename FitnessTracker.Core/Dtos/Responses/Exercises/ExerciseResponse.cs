using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Exercises;

public record ExerciseResponse
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public MuscleGroup MuscleGroup { get; init; }
    public DifficultyLevel DifficultyLevel { get; init; }
}
