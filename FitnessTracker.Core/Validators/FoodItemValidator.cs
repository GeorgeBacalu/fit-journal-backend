using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Core.Services;

namespace FitnessTracker.Core.Validators;

public class FoodItemValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IFoodItemValidator
{
    public async Task ValidateAddAsync(AddFoodItemRequest request, CancellationToken token) =>
        await ValidateAsync(request, excludeId: null, token);

    public async Task ValidateEditAsync(EditFoodItemRequest request, CancellationToken token) =>
        await ValidateAsync(request, excludeId: request.Id, token);

    private async Task ValidateAsync(AddFoodItemRequest request, Guid? excludeId, CancellationToken token)
    {
        if (await _unitOfWork.FoodItems.AnyAsync(fi => fi.Name == request.Name && (excludeId == null || fi.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);
    }
}
