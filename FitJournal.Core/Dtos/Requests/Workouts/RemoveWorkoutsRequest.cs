using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Workouts;

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
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdsRequired.Message)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.Workouts.DuplicatedIds.Message);
    }
}
