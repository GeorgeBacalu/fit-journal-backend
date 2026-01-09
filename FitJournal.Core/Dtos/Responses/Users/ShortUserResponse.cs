using FitJournal.Domain.Enums.Users;

namespace FitJournal.Core.Dtos.Responses.Users;

public record ShortUserResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public DateOnly Birthday { get; init; }
    public decimal Height { get; init; }
    public decimal Weight { get; init; }
    public Gender Gender { get; init; }
}
