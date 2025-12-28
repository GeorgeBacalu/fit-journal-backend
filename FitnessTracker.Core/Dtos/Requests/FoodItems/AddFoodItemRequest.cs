using FitnessTracker.Core.Constants;
using FitnessTracker.Domain.Enums;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record AddFoodItemRequest
{
    public required string Name { get; init; }
    public decimal Calories { get; init; }
    public decimal Protein { get; init; }
    public decimal Carbs { get; init; }
    public decimal Fat { get; init; }
    public FoodCategory Category { get; init; }
    public FoodBrand Brand { get; init; }
}

public class AddFoodItemValidator : AbstractValidator<AddFoodItemRequest>
{
    public AddFoodItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong);

        RuleFor(x => x.Calories)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CaloriesRequired)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidCalories);

        RuleFor(x => x.Protein)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.ProteinRequired)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidProtein);

        RuleFor(x => x.Carbs)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CarbsRequired)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidCarbs);

        RuleFor(x => x.Fat)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.FatRequired)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidFat);

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CategoryRequired)
            .IsInEnum().WithMessage(ValidationErrors.FoodItems.InvalidCategory);

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.BrandRequired)
            .IsInEnum().WithMessage(ValidationErrors.FoodItems.InvalidBrand);
    }
}
