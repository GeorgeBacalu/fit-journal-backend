using FitnessTracker.Domain.Enums.Users;

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

    public ICollection<Workout> Workouts { get; } = [];
    public ICollection<Goal> Goals { get; } = [];
    public ICollection<FoodLog> FoodLogs { get; } = [];
    public ICollection<ProgressLog> ProgressLogs { get; } = [];
    public ICollection<RequestLog> RequestLogs { get; } = [];
}
