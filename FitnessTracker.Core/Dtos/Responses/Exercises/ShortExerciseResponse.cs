using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Exercises;

public class ShortExerciseResponse
{
    public required string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
}
