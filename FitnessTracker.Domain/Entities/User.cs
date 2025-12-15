using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Domain.Entities;

public class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Phone { get; set; }

    public DateOnly Birthday { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }
}
