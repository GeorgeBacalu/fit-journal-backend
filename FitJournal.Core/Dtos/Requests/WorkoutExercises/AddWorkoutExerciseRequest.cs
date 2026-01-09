using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.WorkoutExercises;

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
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Workouts.InvalidId.Message);

        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Exercises.InvalidId.Message);

        RuleFor(x => x.Sets)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.SetsRequired.Message)
            .InclusiveBetween(1, 10).WithMessage(ValidationErrors.WorkoutExercises.SetsOutOfRange.Message);

        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.RepsRequired.Message)
            .InclusiveBetween(1, 50).WithMessage(ValidationErrors.WorkoutExercises.RepsOutOfRange.Message);

        RuleFor(x => x.WeightUsed)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.WeightUsedRequired.Message)
            .InclusiveBetween(0, 500).WithMessage(ValidationErrors.WorkoutExercises.WeightUsedOutOfRange.Message);
    }
}
