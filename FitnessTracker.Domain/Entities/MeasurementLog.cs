namespace FitnessTracker.Domain.Entities;

public class MeasurementLog : BaseEntity, IUserOwnedEntity
{
    public DateOnly Date { get; set; }
    public decimal Weight { get; set; }
    public decimal BodyFatPercentage { get; set; }
    public decimal WaistCircumference { get; set; }
    public decimal ChestCircumference { get; set; }
    public decimal ArmsCircumference { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
