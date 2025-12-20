using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Exercises;

public record ShortExerciseResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public MuscleGroup MuscleGroup { get; init; }
    public DifficultyLevel DifficultyLevel { get; init; }
}
