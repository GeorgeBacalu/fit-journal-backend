using FitnessTracker.App.Attributes;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Auth;

public record RegisterRequest
{
    [Required(ErrorMessage = ValidationConstants.NameRequired)]
    [MaxLength(50, ErrorMessage = ValidationConstants.InvalidNameLength)]
    public required string Name { get; init; }

    [Required(ErrorMessage = ValidationConstants.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationConstants.InvalidEmail)]
    [MaxLength(50, ErrorMessage = ValidationConstants.InvalidEmailLength)]
    public required string Email { get; init; }

    [Required(ErrorMessage = ValidationConstants.PasswordRequired)]
    [RegularExpression(ValidationConstants.PasswordRegex, ErrorMessage = ValidationConstants.InvalidPassword)]
    public required string Password { get; init; }

    [Required(ErrorMessage = ValidationConstants.ConfirmPassword)]
    [Compare(nameof(Password), ErrorMessage = ValidationConstants.PasswordsMismatch)]
    public required string ConfirmedPassword { get; init; }

    [Required(ErrorMessage = ValidationConstants.PhoneRequired)]
    [Phone(ErrorMessage = ValidationConstants.InvalidPhone)]
    [MaxLength(15, ErrorMessage = ValidationConstants.InvalidPhoneLength)]
    public required string Phone { get; init; }

    [Required(ErrorMessage = ValidationConstants.BirthdayRequired)]
    [Past(ErrorMessage = ValidationConstants.InvalidBirthday)]
    public DateOnly Birthday { get; init; }

    [Required(ErrorMessage = ValidationConstants.HeightRequired)]
    [Range(0, 250, ErrorMessage = ValidationConstants.InvalidHeight)]
    public double Height { get; init; }

    [Required(ErrorMessage = ValidationConstants.WeightRequired)]
    [Range(0, 250, ErrorMessage = ValidationConstants.InvalidWeight)]
    public double Weight { get; init; }

    [Required(ErrorMessage = ValidationConstants.GenderRequired)]
    public Gender Gender { get; init; }
}
