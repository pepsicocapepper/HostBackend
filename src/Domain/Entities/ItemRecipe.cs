namespace Domain.Entities;

public class ItemRecipe
{
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}