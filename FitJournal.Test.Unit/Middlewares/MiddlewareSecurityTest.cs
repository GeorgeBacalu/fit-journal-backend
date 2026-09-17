using System.Security.Claims;
using System.Text;
using FitJournal.Api.Helpers;
using FitJournal.Api.Middlewares;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Moq;

namespace FitJournal.Test.Unit.Middlewares;

public class MiddlewareSecurityTest
{
    [Fact]
    public async Task CachingMiddleware_ShouldPreventStorageOfApiResponses()
    {
        var context = new DefaultHttpContext();

        await new CachingMiddleware().InvokeAsync(context, _ => Task.CompletedTask);

        context.Response.Headers[HeaderNames.CacheControl].ToString().Should().Contain("no-store");
        context.Response.Headers[HeaderNames.CacheControl].ToString().Should().Contain("private");
        context.Response.Headers[HeaderNames.Pragma].ToString().Should().Be("no-cache");
        context.Response.Headers[HeaderNames.Vary].ToString().Should().Contain("Authorization");
    }

    [Fact]
    public void JsonHelper_ShouldRedactNestedCredentialsAndTokens()
    {
        var sanitized = JsonHelper.RemoveSensitiveFields(
            """{"password":"p","session":{"accessToken":"a","refreshToken":"r"},"message":"ok"}""");

        sanitized.Should().NotContain("\"p\"").And.NotContain("\"a\"").And.NotContain("\"r\"");
        sanitized.Should().Contain("\"message\":\"ok\"");
        sanitized.Should().Contain("HIDDEN");
    }

    [Fact]
    public async Task LoggingMiddleware_ShouldRedactHeadersAndBodiesAndTrustAuthenticatedClaimsOnly()
    {
        RequestLog? captured = null;
        var logs = new Mock<IRequestLogRepository>();
        logs.Setup(repository => repository.AddAsync(It.IsAny<RequestLog>(), It.IsAny<CancellationToken>()))
            .Callback<RequestLog, CancellationToken>((log, _) => captured = log)
            .Returns(Task.CompletedTask);
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.SetupGet(work => work.RequestLogs).Returns(logs.Object);
        unitOfWork.Setup(work => work.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var services = new ServiceCollection().AddSingleton(unitOfWork.Object).BuildServiceProvider();
        var userId = Guid.NewGuid();
        var context = new DefaultHttpContext
        {
            RequestServices = services,
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("userId", userId.ToString())], "test"))
        };
        context.Request.Method = HttpMethods.Post;
        context.Request.Path = "/api/v1/Auth/login";
        context.Request.ContentType = "application/json";
        context.Request.Headers[HeaderNames.Authorization] = "Bearer sensitive-access-token";
        context.Request.Headers[HeaderNames.Cookie] = "session=sensitive-cookie";
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(
            """{"email":"athlete@example.test","password":"Sensitive1!"}"""));
        context.Response.Body = new MemoryStream();

        await new LoggingMiddleware().InvokeAsync(context, async httpContext =>
        {
            httpContext.Response.Headers[HeaderNames.SetCookie] = "refreshToken=sensitive-refresh-token";
            await httpContext.Response.WriteAsync(
                """{"accessToken":"sensitive-access-token","refreshToken":"sensitive-refresh-token"}""");
        });

        captured.Should().NotBeNull();
        captured!.UserId.Should().Be(userId);
        captured.RequestHeader.Should().Contain("Authorization: HIDDEN").And.Contain("Cookie: HIDDEN");
        captured.ResponseHeader.Should().Contain("Set-Cookie: HIDDEN");
        captured.RequestBody.Should().Contain("HIDDEN").And.NotContain("Sensitive1!");
        captured.ResponseBody.Should().Contain("HIDDEN").And.NotContain("sensitive-access-token");
    }
}
