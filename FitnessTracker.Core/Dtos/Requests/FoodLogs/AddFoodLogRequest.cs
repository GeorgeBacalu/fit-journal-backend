using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodLogs;

public record AddFoodLogRequest
{
    public DateTime? Date { get; init; }
    public int? Servings { get; init; }
    public int? Quantity { get; init; }
    public Guid FoodId { get; init; }
}

public class AddFoodLogValidator : AbstractValidator<AddFoodLogRequest>
{
    public AddFoodLogValidator()
    {
        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodLogs.DateRequired)

            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ValidationErrors.FoodLogs.FutureDate);

        RuleFor(request => request.Servings)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodLogs.ServingsRequired)

            .GreaterThan(0)
            .WithMessage(ValidationErrors.FoodLogs.InvalidServings);

        RuleFor(request => request.Quantity)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodLogs.QuantityRequired)

            .InclusiveBetween(100, 5000)
            .WithMessage(ValidationErrors.FoodLogs.InvalidQuantity);

        RuleFor(request => request.FoodId)
            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.FoodLogs.IdRequired);
    }
}