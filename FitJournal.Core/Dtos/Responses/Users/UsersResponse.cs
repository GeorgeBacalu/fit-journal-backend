namespace FitJournal.Core.Dtos.Responses.Users;

public record UsersResponse
{
    public IEnumerable<ShortUserResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
