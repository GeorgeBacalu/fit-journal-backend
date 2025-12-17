using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record EditExerciseRequest
{
    [Required(ErrorMessage = ValidationErrors.ExerciseIdRequired)]
    public Guid Id { get; init; }

    [Required(ErrorMessage = ValidationErrors.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public string? Name { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidDescriptionLength)]
    public string? Description { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidNotesLength)]
    public string? Notes { get; init; }

    [Required(ErrorMessage = ValidationErrors.MuscleGroupRequired)]
    public MuscleGroup? MuscleGroup { get; set; }

    [Required(ErrorMessage = ValidationErrors.DifficultyLevelRequired)]
    public DifficultyLevel? DifficultyLevel { get; set; }
}
