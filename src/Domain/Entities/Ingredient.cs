using Domain.Common;

namespace Domain.Entities;

public class Ingredient : BaseEntity<int>
{
    public string Name { get; set; } = null!;
    public ICollection<IngredientProvider> ProviderIngredients { get; set; } = [];
    public ICollection<ItemIngredient> RecipeIngredients { get; set; } = [];
    public ICollection<BranchIngredient> BranchIngredients { get; set; } = [];
}