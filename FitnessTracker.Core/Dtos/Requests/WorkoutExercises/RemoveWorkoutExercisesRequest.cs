using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.WorkoutExercises;

public record RemoveWorkoutExercisesRequest
{
    public Guid WorkoutId { get; init; }
    public IEnumerable<Guid> ExerciseIds { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveWorkoutExerciseValidator : AbstractValidator<RemoveWorkoutExercisesRequest>
{
    public RemoveWorkoutExerciseValidator()
    {
        RuleFor(x => x.WorkoutId)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Workouts.InvalidId.Message);

        RuleFor(x => x.ExerciseIds)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.IdsRequired.Message)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.Exercises.DuplicatedIds.Message);
    }
}
