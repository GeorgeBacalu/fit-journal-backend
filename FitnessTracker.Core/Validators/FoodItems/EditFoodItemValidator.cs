using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.FoodItems;

public class EditFoodItemValidator : AbstractValidator<EditFoodItemRequest>
{
    public EditFoodItemValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItemIdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.InvalidFoodItemId);

        Include(new AddFoodItemValidator());
    }
}
