using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Enums.Logging;
using FitnessTracker.Infra.Helpers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Text;

namespace FitnessTracker.Api.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    private const int MaxPayloadSize = 10 * 1024;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;
        context.Request.EnableBuffering();

        Exception? exception = null;
        try
        {
            await next(context);
            buffer.Position = 0;
            await buffer.CopyToAsync(originalBody);
        }
        catch (Exception ex)
        {
            exception ??= ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            var requestBody = await GetRequestBodyAsync(context);
            var responseBody = await GetResponseBodyAsync(buffer);

            context.Response.Body = originalBody;
            try
            {
                var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
                await unitOfWork.RequestLogs.AddAsync(new()
                {
                    Duration = stopwatch.ElapsedMilliseconds,

                    ExceptionType = exception?.GetType().Name,
                    ExceptionMessage = exception?.Message,
                    ExceptionStackTrace = exception?.StackTrace,
                    InnerExceptionType = exception?.InnerException?.GetType().Name,
                    InnerExceptionMessage = exception?.InnerException?.Message,
                    InnerExceptionStackTrace = exception?.InnerException?.StackTrace,

                    Host = $"{context.Request.Host}",
                    Ip = $"{context.Connection.RemoteIpAddress}",
                    Language = context.Request.Headers.ContentLanguage.FirstOrDefault()
                        ?? context.Request.Headers.AcceptLanguage.FirstOrDefault()?.Split(',')[0] ?? "Unknown",

                    Method = Enum.TryParse<MethodType>(context.Request.Method, true, out var method) ? method : null,
                    Path = context.Request.Path,
                    QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,

                    RemoteIp = context.Connection.RemoteIpAddress is { AddressFamily: AddressFamily.InterNetwork } ip
                        ? BitConverter.ToUInt32([.. ip.GetAddressBytes().Reverse()], 0) : 0,

                    RequestBody = requestBody.Length <= MaxPayloadSize ? requestBody : requestBody[..MaxPayloadSize] + "...(truncated)",
                    RequestBodySize = Encoding.UTF8.GetByteCount(requestBody),
                    RequestHeader = string.Join('\n', context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")),

                    ResponseBody = responseBody.Length <= MaxPayloadSize ? responseBody : responseBody[..MaxPayloadSize] + "...(truncated)",
                    ResponseBodySize = Encoding.UTF8.GetByteCount(responseBody),
                    ResponseHeader = string.Join('\n', context.Response.Headers.Select(h => $"{h.Key}: {h.Value}")),
                    ResponseStatus = context.Response.StatusCode,

                    UserId = TryGetUserId(context)
                }, default);
                await unitOfWork.CommitAsync(default);
            }
            catch { /* Logging failure should not impact the main request flow} */ }
        }
    }

    private static async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        if (context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();

            return string.Join('&', form.Keys.Select(k => $"{k}={form[k]}")
                .Concat(form.Files.Select((f, i) => $"file{i}={f.FileName}")));
        }

        context.Request.Body.Position = 0;
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        return string.IsNullOrWhiteSpace(body) ? string.Empty : JsonHelper.RemoveSensitiveFields(body);
    }

    private static async Task<string> GetResponseBodyAsync(Stream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var body = await reader.ReadToEndAsync();
        stream.Position = 0;

        return body ?? string.Empty;
    }

    private static Guid? TryGetUserId(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var header)) return null;

        var auth = header.ToString();
        if (!auth.StartsWith("Bearer ")) return null;

        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(auth["Bearer ".Length..].Trim());
            return Guid.TryParse(jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out var id) ? id : null;
        }
        catch
        {
            return null;
        }
    }
}
