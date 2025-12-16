using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Core.Attributes;

public class PastAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext _)
    {
        if (value == null) return ValidationResult.Success;

        if (value is DateOnly dateOnly)
            return dateOnly > DateOnly.FromDateTime(DateTime.UtcNow)
                ? new(ErrorMessage ?? "Date can't be in the future")
                : ValidationResult.Success;

        if (value is DateTime dateTime)
            return dateTime > DateTime.UtcNow
                ? new(ErrorMessage ?? "Date can't be in the future")
                : ValidationResult.Success;

        return new("Invalid date value");
    }
}
