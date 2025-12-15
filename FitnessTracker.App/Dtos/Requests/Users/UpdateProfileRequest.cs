using FitnessTracker.App.Attributes;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Users;

public record UpdateProfileRequest
{
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public string? Name { get; init; }

    [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidEmailLength)]
    public string? Email { get; init; }

    [Phone(ErrorMessage = ValidationErrors.InvalidPhone)]
    [MaxLength(20, ErrorMessage = ValidationErrors.InvalidPhoneLength)]
    public string? Phone { get; init; }

    [Past(ErrorMessage = ValidationErrors.InvalidBirthday)]
    public DateOnly? Birthday { get; init; }

    [Range(0, 250, ErrorMessage = ValidationErrors.InvalidHeight)]
    public double? Height { get; init; }

    [Range(0, 250, ErrorMessage = ValidationErrors.InvalidWeight)]
    public double? Weight { get; init; }

    public Gender? Gender { get; init; }
}
