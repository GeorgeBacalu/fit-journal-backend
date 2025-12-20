using FitnessTracker.Infra.Constants;
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
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.IdRequired)

            .Must(id => id != default)
            .WithMessage(ValidationErrors.FoodItems.InvalidId);

        Include(new AddFoodItemValidator());
    }
}
