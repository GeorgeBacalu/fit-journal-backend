using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodLogs;

public record EditFoodLogRequest : AddFoodLogRequest
{
    public Guid Id { get; init; }
}

public class EditFoodLogValidator : AbstractValidator<EditFoodLogRequest>
{
    public EditFoodLogValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.FoodLogs.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.FoodLogs.InvalidId.Message);

        Include(new AddFoodLogValidator());
    }
}
