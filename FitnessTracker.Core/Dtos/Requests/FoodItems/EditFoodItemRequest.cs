using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record EditFoodItemRequest : AddFoodItemRequest
{
    public Guid Id { get; init; }
}

public class EditFoodItemValidator : AbstractValidator<EditFoodItemRequest>
{
    public EditFoodItemValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.FoodItems.InvalidId.Message);

        Include(new AddFoodItemValidator());
    }
}
