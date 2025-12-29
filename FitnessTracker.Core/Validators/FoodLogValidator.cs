using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Core.Services;

namespace FitnessTracker.Core.Validators;

public class FoodLogValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IFoodLogValidator
{
    public async Task ValidateAddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, token);

    public async Task ValidateEditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token) =>
        await ValidateAsync(request, userId, token);

    private async Task ValidateAsync(AddFoodLogRequest request, Guid userId, CancellationToken token)
    {
        if (await _unitOfWork.FoodItems.GetByIdTrackedAsync(request.FoodId, token) == null)
            throw new NotFoundException(BusinessErrors.FoodLogs.IdNotFound(request.FoodId));

        var user = await _unitOfWork.Users.GetByIdTrackedAsync(userId, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(userId));

        if (request.Date < user.CreatedAt)
            throw new BadRequestException(ValidationErrors.FoodLogs.BeforeRegistration);
    }
}
