using FitJournal.Core.Results;

namespace FitJournal.Core.Exceptions;

public class BadRequestException(Error error) : AppException(error);
