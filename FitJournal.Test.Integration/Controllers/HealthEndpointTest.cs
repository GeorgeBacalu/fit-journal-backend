using FitJournal.Test.Integration.Config;
using FluentAssertions;
using System.Net;

namespace FitJournal.Test.Integration.Controllers;

public class HealthEndpointTest(DbFixture fixture) : IClassFixture<DbFixture>
{
    [Theory]
    [InlineData("/health/live")]
    [InlineData("/health/ready")]
    public async Task HealthEndpoint_ReturnsOk(string path)
    {
        await using var factory = new WebAppFactory(fixture);
        using var client = factory.CreateClient();

        var response = await client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
