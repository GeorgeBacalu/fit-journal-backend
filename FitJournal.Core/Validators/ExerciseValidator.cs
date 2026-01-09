using AutoMapper;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;

namespace FitJournal.Core.Validators;

public class ExerciseValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IExerciseValidator
{
    public async Task ValidateAddAsync(AddExerciseRequest request, CancellationToken token) =>
        await ValidateAsync(request, excludeId: null, token);

    public async Task ValidateEditAsync(EditExerciseRequest request, CancellationToken token) =>
        await ValidateAsync(request, excludeId: request.Id, token);

    private async Task ValidateAsync(AddExerciseRequest request, Guid? excludeId, CancellationToken token)
    {
        if (await _unitOfWork.Exercises.AnyAsync(e => e.Name == request.Name && (excludeId == null || e.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);
    }
}
