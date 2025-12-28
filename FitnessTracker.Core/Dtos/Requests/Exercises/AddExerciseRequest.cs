using FitnessTracker.Core.Constants;
using FitnessTracker.Domain.Enums;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record AddExerciseRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public MuscleGroup MuscleGroup { get; init; }
    public DifficultyLevel DifficultyLevel { get; init; }
}

public class AddExerciseValidator : AbstractValidator<AddExerciseRequest>
{
    public AddExerciseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(ValidationErrors.Exercises.InvalidDescriptionLength);

        RuleFor(x => x.Notes)
            .MaximumLength(250).WithMessage(ValidationErrors.Exercises.InvalidNotesLength);

        RuleFor(x => x.MuscleGroup)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.MuscleGroupRequired)
            .IsInEnum().WithMessage(ValidationErrors.Exercises.InvalidMuscleGroup);

        RuleFor(x => x.DifficultyLevel)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.DifficultyLevelRequired)
            .IsInEnum().WithMessage(ValidationErrors.Exercises.InvalidDifficultyLevel);
    }
}
