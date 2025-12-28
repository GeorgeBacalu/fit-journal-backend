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
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.IdRequired)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.FoodItems.InvalidId);

        Include(new AddFoodItemValidator());
    }
}
