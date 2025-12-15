using FitnessTracker.App.Attributes;
using FitnessTracker.Infra.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Workouts;

public class EditWorkoutRequest
{
    [Required(ErrorMessage = ValidationErrors.WorkoutIdRequired)]
    public Guid Id { get; init; }

    [Required(ErrorMessage = ValidationErrors.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public string? Name { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidDescriptionLength)]
    public string? Description { get; init; }

    [MaxLength(250, ErrorMessage = ValidationErrors.InvalidNotesLength)]
    public string? Notes { get; init; }

    [Range(5, 300, ErrorMessage = ValidationErrors.InvalidDuration)]
    public int? DurationMinutes { get; init; }

    [Past(ErrorMessage = ValidationErrors.InvalidWorkoutDate)]
    public DateTime? StartedAt { get; init; }
}
