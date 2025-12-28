using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.WorkoutExercises;

public record AddWorkoutExerciseRequest
{
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }

    public int Sets { get; init; }
    public int Reps { get; init; }
    public decimal WeightUsed { get; init; }
}

public class AddWorkoutExerciseValidator : AbstractValidator<AddWorkoutExerciseRequest>
{
    public AddWorkoutExerciseValidator()
    {
        RuleFor(x => x.WorkoutId)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdRequired)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Workouts.InvalidId);

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.IdRequired)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Exercises.InvalidId);

        RuleFor(x => x.Sets)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.SetsRequired)
            .InclusiveBetween(1, 10).WithMessage(ValidationErrors.WorkoutExercises.SetsOutOfRange);

        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.RepsRequired)
            .InclusiveBetween(1, 50).WithMessage(ValidationErrors.WorkoutExercises.RepsOutOfRange);

        RuleFor(x => x.WeightUsed)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.WeightUsedRequired)
            .InclusiveBetween(0, 500).WithMessage(ValidationErrors.WorkoutExercises.WeightUsedOutOfRange);
    }
}
