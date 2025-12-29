using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodLogs;

public record AddFoodLogRequest
{
    public DateTime Date { get; init; }
    public int Servings { get; init; }
    public decimal Quantity { get; init; }

    public Guid FoodId { get; init; }
}

public class AddFoodLogValidator : AbstractValidator<AddFoodLogRequest>
{
    public AddFoodLogValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage(ValidationErrors.FoodLogs.DateRequired.Message)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ValidationErrors.FoodLogs.FutureDate.Message);

        RuleFor(x => x.Servings)
            .NotEmpty().WithMessage(ValidationErrors.FoodLogs.ServingsRequired.Message)
            .GreaterThan(0).WithMessage(ValidationErrors.FoodLogs.InvalidServings.Message);

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(ValidationErrors.FoodLogs.QuantityRequired.Message)
            .InclusiveBetween(100, 5000).WithMessage(ValidationErrors.FoodLogs.QuantityOutOfRange.Message);

        RuleFor(x => x.FoodId)
            .NotEmpty().WithMessage(ValidationErrors.FoodItems.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.FoodLogs.IdRequired.Message);
    }
}
