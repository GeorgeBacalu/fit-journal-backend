using FitJournal.Core.Dtos.Common.Email;
using FitJournal.Core.Dtos.Requests.Email;

namespace FitJournal.Core.Interfaces.Services;

public interface IEmailService : IBusinessService
{
    string GeneratePasswordResetEmail(PasswordResetEmail data);

    Task SendAsync(SendEmailRequest request, CancellationToken token);
}
