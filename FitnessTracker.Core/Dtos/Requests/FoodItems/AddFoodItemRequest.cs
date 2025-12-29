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
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong.Message);

        RuleFor(x => x.Calories)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CaloriesRequired.Message)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidCalories.Message);

        RuleFor(x => x.Protein)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.ProteinRequired.Message)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidProtein.Message);

        RuleFor(x => x.Carbs)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CarbsRequired.Message)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidCarbs.Message);

        RuleFor(x => x.Fat)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.FatRequired.Message)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodItems.InvalidFat.Message);

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.CategoryRequired.Message)
            .IsInEnum().WithMessage(ValidationErrors.FoodItems.InvalidCategory.Message);

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.BrandRequired.Message)
            .IsInEnum().WithMessage(ValidationErrors.FoodItems.InvalidBrand.Message);
    }
}
