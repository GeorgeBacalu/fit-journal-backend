namespace FitnessTracker.Core.Dtos.Requests.FoodLogs;

public class RemoveFoodLogsValidator : AbstractValidator<RemoveFoodLogsRequest>
{
    public RemoveFoodLogsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.FoodLogs.IdsRequired)
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.FoodLogs.DuplicatedIds);
    }
}