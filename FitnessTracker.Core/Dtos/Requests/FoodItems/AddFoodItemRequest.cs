using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record AddFoodItemRequest
{
    public string? Name { get; init; }
    public decimal? Calories { get; init; }
    public decimal? Protein { get; init; }
    public decimal? Carbs { get; init; }
    public decimal? Fat { get; init; }
}

public class AddFoodItemValidator : AbstractValidator<AddFoodItemRequest>
{
    public AddFoodItemValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.FoodItems.InvalidNameLength);

        RuleFor(request => request.Calories)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.CaloriesRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.FoodItems.InvalidCalories);

        RuleFor(request => request.Protein)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.ProteinRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.FoodItems.InvalidProtein);

        RuleFor(request => request.Carbs)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.CarbsRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.FoodItems.InvalidCarbs);

        RuleFor(request => request.Fat)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodItems.FatRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.FoodItems.InvalidFat);
    }
}
