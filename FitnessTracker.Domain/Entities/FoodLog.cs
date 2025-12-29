namespace FitnessTracker.Domain.Entities;

public class FoodLog : BaseEntity, IUserOwnedEntity
{
    public DateTime Date { get; set; }
    public int Servings { get; set; }
    public decimal Quantity { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid FoodId { get; set; }
    public FoodItem? Food { get; set; }
}
