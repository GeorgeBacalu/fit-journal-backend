using FitnessTracker.Core.Dtos.Requests.Goals;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Goals;

public class RemoveGoalsValidator: AbstractValidator<RemoveGoalsRequest>
{
    public RemoveGoalsValidator()
    {
    }
}
