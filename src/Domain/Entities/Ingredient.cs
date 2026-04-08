using Domain.Common;

namespace Domain.Entities;

public class Ingredient : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public Guid ProviderId { get; set; }
    public ICollection<IngredientProvider> ProviderIngredients { get; set; } = [];
    public ICollection<ItemIngredient> RecipeIngredients { get; set; } = [];
}