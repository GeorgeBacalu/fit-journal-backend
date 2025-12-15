using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.App.Attributes;

public class PastAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext _)
    {
        if (value == null) return ValidationResult.Success;
        if (value is not DateOnly date) return new("Invalid data value");

        return date > DateOnly.FromDateTime(DateTime.UtcNow)
            ? new(ErrorMessage ?? "Date can't be in the future")
            : ValidationResult.Success;
    }
}
