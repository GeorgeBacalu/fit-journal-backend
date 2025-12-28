using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Core.Services;

namespace FitnessTracker.Core.Validators;

public class ProgressLogValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IProgressLogValidator
{
    public async Task ValidateAddAsync(AddProgressLogRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: null, token);

    public async Task ValidateEditAsync(EditProgressLogRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: request.Id, token);

    private async Task ValidateAsync(AddProgressLogRequest request, Guid userId, Guid? excludeId, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(userId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Users.IdNotFound, userId));

        if (request.Date < DateOnly.FromDateTime(user.CreatedAt))
            throw new BadRequestException(ValidationErrors.ProgressLogs.BeforeRegistration);

        if (await _unitOfWork.ProgressLogs.AnyAsync(pl => pl.UserId == userId && pl.Date == request.Date && (excludeId == null || pl.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.ProgressLogs.DuplicatedDailyLog);

        var lastLog = await _unitOfWork.ProgressLogs.GetLastAsync(userId, token);
        if (lastLog != null)
        {
            var days = request.Date.DayNumber - lastLog.Date.DayNumber;
            if (days > 0 && Math.Abs(request.Weight - lastLog.Weight) > 2 * days)
                throw new BadRequestException(ValidationErrors.ProgressLogs.WeightChangeTooHigh);
        }
    }
}
