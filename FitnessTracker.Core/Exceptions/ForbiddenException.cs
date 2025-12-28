using FitnessTracker.Core.Results;

namespace FitnessTracker.Core.Exceptions;

public class ForbiddenException(Error error) : AppException(error);
