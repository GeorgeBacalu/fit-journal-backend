using FitnessTracker.Domain.Enums.FoodItems;

namespace FitnessTracker.Domain.Entities;

public class FoodItem : BaseEntity
{
    public required string Name { get; set; }
    public FoodCategory Category { get; set; }
    public FoodBrand Brand { get; set; }
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }

    public ICollection<FoodLog> FoodLogs { get; } = [];
}
