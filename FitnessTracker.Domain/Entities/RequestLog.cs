using FitnessTracker.Domain.Enums.Logging;

namespace FitnessTracker.Domain.Entities;

public class RequestLog : BaseEntity
{
    public long Duration { get; set; }

    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public string? InnerExceptionType { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public string? InnerExceptionStackTrace { get; set; }

    public required string Host { get; set; }
    public required string Ip { get; set; }
    public required string Language { get; set; }

    public MethodType? Method { get; set; }
    public required string Path { get; set; }
    public string? QueryString { get; set; }

    public long RemoteIp { get; set; }

    public required string RequestBody { get; set; }
    public int RequestBodySize { get; set; }
    public required string RequestHeader { get; set; }
    public required string ResponseBody { get; set; }

    public int ResponseBodySize { get; set; }
    public required string ResponseHeader { get; set; }
    public int ResponseStatus { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}
