using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Domain.Entities;

public class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Phone { get; set; }
    public DateOnly Birthday { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public Gender Gender { get; set; }
    public Role Role { get; set; }

    public IEnumerable<Workout> Workouts { get; } = [];
    public IEnumerable<Goal> Goals { get; } = [];
    public IEnumerable<FoodLog> FoodLogs { get; } = [];
    public IEnumerable<MeasurementLog> MeasurementLogs { get; } = [];
}
