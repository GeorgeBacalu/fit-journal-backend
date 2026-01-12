using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IResetTokenRepository : IUserOwnedRepository<ResetToken>
{
    Task<ResetToken?> GetLastAsync(Guid userId, CancellationToken token);
}
