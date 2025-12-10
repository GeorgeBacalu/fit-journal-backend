using FitnessTracker.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Dtos.Requests.Auth;

public record LoginRequest
{
    [Required(ErrorMessage = ValidationConstants.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationConstants.InvalidEmail)]
    [MaxLength(50, ErrorMessage = ValidationConstants.InvalidEmailLength)]
    public required string Email { get; init; }

    [Required(ErrorMessage = ValidationConstants.PasswordRequired)]
    public required string Password { get; init; }
}
