using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record RemoveWorkoutsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}

public class RemoveWorkoutsValidator : AbstractValidator<RemoveWorkoutsRequest>
{
    public RemoveWorkoutsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.Workouts.IdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.Workouts.DuplicatedIds);
    }
}
