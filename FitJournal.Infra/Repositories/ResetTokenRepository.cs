using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;

namespace FitJournal.Infra.Repositories;

public class ResetTokenRepository(AppDbContext db)
    : UserOwnedRepository<ResetToken>(db), IResetTokenRepository
{
}
