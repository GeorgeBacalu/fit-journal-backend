using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.FoodItems;

public class AddFoodItemValidator : AbstractValidator<AddFoodItemRequest>
{
    public AddFoodItemValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidNameLength);

        RuleFor(request => request.Calories)
            .NotEmpty()
            .WithMessage(ValidationErrors.CaloriesRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.InvalidCalories);

        RuleFor(request => request.Protein)
            .NotEmpty()
            .WithMessage(ValidationErrors.ProteinRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.InvalidProtein);

        RuleFor(request => request.Carbs)
            .NotEmpty()
            .WithMessage(ValidationErrors.CarbsRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.InvalidCarbs);

        RuleFor(request => request.Fat)
            .NotEmpty()
            .WithMessage(ValidationErrors.FatRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.InvalidFat);
    }
}
