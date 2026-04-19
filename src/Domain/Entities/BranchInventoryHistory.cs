using Domain.Common;

namespace Domain.Entities;

public class BranchInventoryHistory : BaseEntity<int>
{
    public decimal PreviousQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    public DateTime Date { get; set; }
    public BranchInventoryAction Action { get; set; }
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}