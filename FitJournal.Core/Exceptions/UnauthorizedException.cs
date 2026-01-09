using FitJournal.Core.Results;

namespace FitJournal.Core.Exceptions;

public class UnauthorizedException(Error error) : AppException(error);
