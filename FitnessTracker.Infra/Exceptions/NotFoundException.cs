namespace FitnessTracker.Infra.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}
