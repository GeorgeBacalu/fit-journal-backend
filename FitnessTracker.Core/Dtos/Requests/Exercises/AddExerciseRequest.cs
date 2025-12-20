using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record AddExerciseRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public MuscleGroup? MuscleGroup { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }
}

public class AddExerciseValidator : AbstractValidator<AddExerciseRequest>
{
    public AddExerciseValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.Exercises.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Exercises.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.Exercises.InvalidDescriptionLength);

        RuleFor(request => request.Notes)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.Exercises.InvalidNotesLength);

        RuleFor(request => request.MuscleGroup)
            .NotEmpty()
            .WithMessage(ValidationErrors.Exercises.MuscleGroupRequired);

        RuleFor(request => request.DifficultyLevel)
            .NotEmpty()
            .WithMessage(ValidationErrors.Exercises.DifficultyLevelRequired);
    }
}
