using FitnessTracker.Core.Results;

namespace FitnessTracker.Core.Exceptions;

public class NotFoundException(Error error) : AppException(error);
