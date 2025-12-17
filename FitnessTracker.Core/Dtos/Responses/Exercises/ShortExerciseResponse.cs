using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Exercises;

public record ShortExerciseResponse
{
    public required string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
}
