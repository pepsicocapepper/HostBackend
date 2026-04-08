using Domain.Common;

namespace Domain.Entities;

public class ItemIngredient
{
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public int Quantity { get; set; }
}