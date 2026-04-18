namespace Domain.Entities;

public class BranchIngredient
{
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;
}