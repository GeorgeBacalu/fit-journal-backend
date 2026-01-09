using FitJournal.Core.Results;

namespace FitJournal.Core.Exceptions;

public abstract class AppException(Error error) : Exception(error.Message)
{
    public Error Error { get; } = error;
}
