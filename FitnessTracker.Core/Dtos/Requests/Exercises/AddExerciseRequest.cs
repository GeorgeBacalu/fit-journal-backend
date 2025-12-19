using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record AddExerciseRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public MuscleGroup? MuscleGroup { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }
}
