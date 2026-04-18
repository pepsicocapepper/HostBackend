using Domain.Common;

namespace Domain.Entities;

public class Recipe : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public List<string> Steps { get; set; } = [];
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
    public ICollection<ItemRecipe> ItemRecipes { get; set; } = [];
}