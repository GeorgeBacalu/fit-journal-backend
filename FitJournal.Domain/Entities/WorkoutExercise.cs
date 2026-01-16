namespace FitJournal.Domain.Entities;

public class WorkoutExercise : BaseEntity
{
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal WeightUsed { get; set; }

    public Guid WorkoutId { get; set; }
    public Workout? Workout { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
}
