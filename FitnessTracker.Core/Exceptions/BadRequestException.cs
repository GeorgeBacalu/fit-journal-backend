using FitnessTracker.Core.Results;

namespace FitnessTracker.Core.Exceptions;

public class BadRequestException(Error error) : AppException(error);
