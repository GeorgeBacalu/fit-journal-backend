using FitnessTracker.App.Attributes;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Auth;

public record RegisterRequest
{
    [Required(ErrorMessage = ValidationErrors.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidNameLength)]
    public required string Name { get; init; }

    [Required(ErrorMessage = ValidationErrors.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidEmailLength)]
    public required string Email { get; init; }

    [Required(ErrorMessage = ValidationErrors.PasswordRequired)]
    [RegularExpression(ValidationRules.PasswordRegex, ErrorMessage = ValidationErrors.InvalidPassword)]
    public required string Password { get; init; }

    [Required(ErrorMessage = ValidationErrors.ConfirmPassword)]
    [Compare(nameof(Password), ErrorMessage = ValidationErrors.PasswordsMismatch)]
    public required string ConfirmedPassword { get; init; }

    [Required(ErrorMessage = ValidationErrors.PhoneRequired)]
    [Phone(ErrorMessage = ValidationErrors.InvalidPhone)]
    [MaxLength(20, ErrorMessage = ValidationErrors.InvalidPhoneLength)]
    public required string Phone { get; init; }

    [Required(ErrorMessage = ValidationErrors.BirthdayRequired)]
    [Past(ErrorMessage = ValidationErrors.InvalidBirthday)]
    public DateOnly? Birthday { get; init; }

    [Required(ErrorMessage = ValidationErrors.HeightRequired)]
    [Range(0, 250, ErrorMessage = ValidationErrors.InvalidHeight)]
    public double? Height { get; init; }

    [Required(ErrorMessage = ValidationErrors.WeightRequired)]
    [Range(0, 250, ErrorMessage = ValidationErrors.InvalidWeight)]
    public double? Weight { get; init; }

    [Required(ErrorMessage = ValidationErrors.GenderRequired)]
    public Gender? Gender { get; init; }
}
