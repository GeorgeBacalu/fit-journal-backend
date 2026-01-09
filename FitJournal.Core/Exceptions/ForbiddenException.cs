using FitJournal.Core.Results;

namespace FitJournal.Core.Exceptions;

public class ForbiddenException(Error error) : AppException(error);
