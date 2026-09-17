using AutoMapper;
using FitJournal.Core.Dtos.Common.Email;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Services;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class EmailServiceTest
{
    [Fact]
    public void GeneratePasswordResetEmail_IncludesRecipientLinkAndUtcExpiry()
    {
        var service = new EmailService(Mock.Of<IUnitOfWork>(), Mock.Of<IMapper>());
        var data = new PasswordResetEmail
        {
            UserName = "Alex",
            ResetLink = "https://fitjournal.test/reset?token=abc",
            ExpiresAt = new DateTime(2026, 9, 17, 12, 30, 0, DateTimeKind.Utc)
        };

        var body = service.GeneratePasswordResetEmail(data);

        body.Should().Contain("Hello Alex");
        body.Should().Contain("href='https://fitjournal.test/reset?token=abc'");
        body.Should().Contain("September 17, 2026 12:30 UTC");
        body.Should().Contain("If you didn't request this");
    }
}
