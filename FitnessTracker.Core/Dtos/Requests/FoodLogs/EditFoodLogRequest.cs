using FitnessTracker.Infra.Constants;
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
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodLogs.IdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.FoodLogs.InvalidId);

        Include(new AddFoodLogValidator());
    }
}
