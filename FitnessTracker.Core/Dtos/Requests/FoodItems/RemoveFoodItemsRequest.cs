using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record RemoveFoodItemsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveFoodItemsValidator : AbstractValidator<RemoveFoodItemsRequest>
{
    public RemoveFoodItemsValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.IdsRequired)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.FoodItems.DuplicatedIds);
    }
}
