using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.FoodItems;

public class RemoveFoodItemsValidator : AbstractValidator<RemoveFoodItemsRequest>
{
    public RemoveFoodItemsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItemIdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.DuplicatedFoodItemIds);
    }
}
