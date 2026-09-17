using Azure.Core;
using FitJournal.Api.Controllers;
using FitJournal.Infra.Context;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FitJournal.Test.Integration.Controllers;

public class AthleteAdviceControllerTest
{
    [Fact]
    public async Task AskAsync_UsesManagedIdentityAndReturnsContextualAdvice()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();

        var handler = new RecordingHandler();
        var client = new HttpClient(handler);
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AzureOpenAI:Endpoint"] = "https://ai.test",
            ["AzureOpenAI:Deployment"] = "fitjournal"
        }).Build();
        var userId = Guid.NewGuid();
        var controller = new AthleteAdviceController(new TestHttpClientFactory(client), configuration, db, new TestTokenCredential())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("userId", userId.ToString())], "test"))
                }
            }
        };

        var result = (await controller.AskAsync(new("How should I recover?"), default)).Result as OkObjectResult;

        result.Should().NotBeNull();
        handler.Authorization.Should().Be("Bearer managed-identity-token");
        handler.RequestBody.Should().Contain("FITJOURNAL_DATA_BEGIN").And.Contain("How should I recover?");
    }

    private sealed class TestHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class TestTokenCredential : TokenCredential
    {
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken) =>
            new("managed-identity-token", DateTimeOffset.UtcNow.AddMinutes(5));

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken) =>
            ValueTask.FromResult(GetToken(requestContext, cancellationToken));
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public string? Authorization { get; private set; }
        public string RequestBody { get; private set; } = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Authorization = request.Headers.Authorization?.ToString();
            RequestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return new(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"choices\":[{\"message\":{\"content\":\"Rest and hydrate.\"}}]}", Encoding.UTF8, "application/json")
            };
        }
    }
}
