namespace FitnessTracker.Core.Interfaces.Services.Admin;

public interface IAdminAuthService : IBusinessService
{
    Task DeleteAsync(Guid id, CancellationToken token);
}
