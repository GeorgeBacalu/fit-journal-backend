using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record RemoveExercisesRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}

public class RemoveExercisesValidator : AbstractValidator<RemoveExercisesRequest>
{
    public RemoveExercisesValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.Exercises.IdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.Exercises.DuplicatedIds);
    }
}
