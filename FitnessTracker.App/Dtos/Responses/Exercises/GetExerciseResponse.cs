using FitnessTracker.Domain.Enums;

namespace FitnessTracker.App.Dtos.Responses.Exercises;

public class GetExerciseResponse
{
    public required string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
}
