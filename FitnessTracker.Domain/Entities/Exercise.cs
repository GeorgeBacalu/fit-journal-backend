using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Domain.Entities;

public class Exercise : BaseEntity
{
    public required string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }

    public IEnumerable<WorkoutExercise> WorkoutExercises { get; set; } = [];
}
