namespace FitnessTracker.Domain.Entities;

public class Workout : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime Date { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
