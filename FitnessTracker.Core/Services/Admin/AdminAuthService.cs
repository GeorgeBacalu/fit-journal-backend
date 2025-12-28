using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;

namespace FitnessTracker.Core.Services.Admin;

public class AdminAuthService(IUnitOfWork unitOfWork) : IAdminAuthService
{
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Users.IdNotFound, id));

        await unitOfWork.Users.RemoveAsync(user, hardDelete: false, token);
        await unitOfWork.CommitAsync(token);
    }
}
