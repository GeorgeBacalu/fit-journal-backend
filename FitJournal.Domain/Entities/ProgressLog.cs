namespace FitJournal.Domain.Entities;

public class ProgressLog : BaseEntity, IUserOwnedEntity
{
    public DateOnly Date { get; set; }
    public decimal Weight { get; set; }
    public decimal BodyFat { get; set; }
    public decimal WaistCm { get; set; }
    public decimal ChestCm { get; set; }
    public decimal ArmsCm { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
