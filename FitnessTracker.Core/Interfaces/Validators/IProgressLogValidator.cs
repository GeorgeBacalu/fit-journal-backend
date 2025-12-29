using FitnessTracker.Core.Dtos.Requests.ProgressLogs;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IProgressLogValidator
{
    Task ValidateAddAsync(AddProgressLogRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditProgressLogRequest request, Guid userId, CancellationToken token);
}
