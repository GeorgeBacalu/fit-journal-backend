using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class ResetTokenRepository(AppDbContext db)
    : UserOwnedRepository<ResetToken>(db), IResetTokenRepository
{
    public async Task<ResetToken?> GetLastAsync(Guid userId, CancellationToken token) =>
        await _db.ResetTokens.Where(rt => rt.UserId == userId)
            .OrderByDescending(rt => rt.CreatedAt)
            .FirstOrDefaultAsync(token);
}
