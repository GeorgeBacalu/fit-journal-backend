using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;

namespace FitnessTracker.Core.Services.Admin;

public class AdminAuthService(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IAdminAuthService
{
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        await _unitOfWork.Users.RemoveAsync(user, hardDelete: false, token);
        await _unitOfWork.CommitAsync(token);
    }
}
