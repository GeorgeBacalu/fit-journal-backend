using FitJournal.Domain.Enums.Exercises;

namespace FitJournal.Domain.Entities;

public class Exercise : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }

    public ICollection<WorkoutExercise> WorkoutExercises { get; } = [];
}
