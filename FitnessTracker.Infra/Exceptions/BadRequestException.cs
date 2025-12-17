namespace FitnessTracker.Infra.Exceptions;

public class BadRequestException(string message)
    : Exception(message) { }
