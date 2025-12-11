using FitnessTracker.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Auth;

public record LoginRequest
{
    [Required(ErrorMessage = ValidationErrors.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
    [MaxLength(50, ErrorMessage = ValidationErrors.InvalidEmailLength)]
    public required string Email { get; init; }

    [Required(ErrorMessage = ValidationErrors.PasswordRequired)]
    public required string Password { get; init; }
}
