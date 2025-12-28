using FitnessTracker.Core.Results;

namespace FitnessTracker.Core.Exceptions;

public class UnauthorizedException(Error error) : AppException(error);
