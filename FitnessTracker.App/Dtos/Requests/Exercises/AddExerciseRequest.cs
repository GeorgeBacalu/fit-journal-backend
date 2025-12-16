using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Exercises;

public record AddExerciseRequest
{
    [Required(ErrorMessage = ValidationErrors.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public required string Name { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidDescriptionLength)]
    public required string Description { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidNotesLength)]
    public required string Notes { get; init; }

    [Required(ErrorMessage = ValidationErrors.MuscleGroupRequired)]
    public MuscleGroup MuscleGroup { get; set; }

    [Required(ErrorMessage = ValidationErrors.DifficultyLevelRequired)]
    public DifficultyLevel DifficultyLevel { get; set; }
}
