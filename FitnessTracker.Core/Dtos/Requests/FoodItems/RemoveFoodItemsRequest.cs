using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record RemoveFoodItemsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}

public class RemoveFoodItemsValidator : AbstractValidator<RemoveFoodItemsRequest>
{
    public RemoveFoodItemsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.IdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.FoodItems.DuplicatedIds);
    }
}
