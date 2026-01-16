using FitJournal.Core.Results;

namespace FitJournal.Core.Exceptions;

public class NotFoundException(Error error) : AppException(error);
