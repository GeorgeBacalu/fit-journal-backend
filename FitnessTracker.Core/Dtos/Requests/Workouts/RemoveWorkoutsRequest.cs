using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record RemoveWorkoutsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveWorkoutsValidator : AbstractValidator<RemoveWorkoutsRequest>
{
    public RemoveWorkoutsValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdsRequired)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.Workouts.DuplicatedIds);
    }
}
