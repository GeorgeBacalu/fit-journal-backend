using AutoMapper;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;

namespace FitJournal.Core.Validators;

public class WorkoutValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IWorkoutValidator
{
    public async Task ValidateAddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: null, token);

    public async Task ValidateEditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: request.Id, token);

    private async Task ValidateAsync(AddWorkoutRequest request, Guid userId, Guid? excludeId, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(userId, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(userId));

        if (request.StartedAt < user.CreatedAt)
            throw new BadRequestException(ValidationErrors.Workouts.BeforeRegistration);

        if (await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Name == request.Name && (excludeId == null || w.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        var start = request.StartedAt.AddTicks(-(request.StartedAt.Ticks % TimeSpan.TicksPerMinute));

        if (await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.StartedAt >= start && w.StartedAt < start.AddMinutes(1) && (excludeId == null || w.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Workouts.DuplicatedStartedAt);
    }
}
