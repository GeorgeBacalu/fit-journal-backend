using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FitJournal.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/athlete-advice")]
public class AthleteAdviceController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : BaseController
{
    [HttpPost]
    public async Task<ActionResult<object>> AskAsync([FromBody] AdviceRequest request, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(request.Question)) return BadRequest("A question is required.");
        var endpoint = configuration["AzureOpenAI:Endpoint"];
        var deployment = configuration["AzureOpenAI:Deployment"];
        var apiKey = configuration["AzureOpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(deployment) || string.IsNullOrWhiteSpace(apiKey))
            return Problem("The athlete-advice agent is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("api-key", apiKey);
        var body = new { messages = new[] { new { role = "system", content = "You are FitJournal's athlete-advice assistant. Give practical, safe, non-diagnostic fitness and nutrition guidance. Encourage a qualified professional for pain, injury, pregnancy, eating disorders, or medical conditions." }, new { role = "user", content = request.Question } }, temperature = 0.4, max_tokens = 400 };
        using var response = await client.PostAsync($"{endpoint.TrimEnd('/')}/openai/deployments/{deployment}/chat/completions?api-version=2024-10-21", new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"), token);
        if (!response.IsSuccessStatusCode) return Problem("The advice service did not return a response.", statusCode: (int)response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(token));
        var answer = document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        return Ok(new { answer });
    }
}

public record AdviceRequest(string Question);
