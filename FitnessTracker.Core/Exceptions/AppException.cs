using FitnessTracker.Core.Results;

namespace FitnessTracker.Core.Exceptions;

public abstract class AppException(Error error) : Exception(error.Message)
{
    public Error Error { get; } = error;
}
