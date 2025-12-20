namespace FitnessTracker.Domain.Entities;

public class FoodLog : BaseEntity
{
    public DateTime Date { get; set; }
    public int Servings { get; set; }
    public int Quantity { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid FoodId { get; set; }
    public FoodItem? Food { get; set; }
}
