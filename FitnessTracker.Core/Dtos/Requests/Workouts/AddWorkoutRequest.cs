using FitnessTracker.Core.Attributes;
using FitnessTracker.Infra.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record AddWorkoutRequest
{
    [Required(ErrorMessage = ValidationErrors.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public required string Name { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidDescriptionLength)]
    public required string Description { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidNotesLength)]
    public required string Notes { get; init; }

    [Range(5, 300, ErrorMessage = ValidationErrors.InvalidDuration)]
    public int DurationMinutes { get; init; }

    [Past(ErrorMessage = ValidationErrors.InvalidWorkoutDate)]
    public DateTime StartedAt { get; init; }

    public Guid? UserId { get; set; }
}
