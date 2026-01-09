using AutoMapper;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Enums.Goals;

namespace FitJournal.Core.Validators;

public class GoalValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IGoalValidator
{
    public async Task ValidateAddAsync(AddGoalRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: null, token);

    public async Task ValidateEditAsync(EditGoalRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, excludeId: request.Id, token);

    private async Task ValidateAsync(AddGoalRequest request, Guid userId, Guid? excludeId, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(userId, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(userId));

        if (request.StartDate < DateOnly.FromDateTime(user.CreatedAt))
            throw new BadRequestException(ValidationErrors.Goals.BeforeRegistration);

        if (request.Type == GoalType.WeightLoss && request.TargetWeight >= user.Weight || request.Type == GoalType.WeightGain && request.TargetWeight <= user.Weight)
            throw new BadRequestException(ValidationErrors.Goals.WeightTargetTypeMismatch);

        if (await _unitOfWork.Goals.AnyAsync(g => g.UserId == userId && g.Type == request.Type && g.StartDate <= request.EndDate && g.EndDate >= request.StartDate && (excludeId == null || g.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Goals.SameTypeGoalOverlap);
    }
}
