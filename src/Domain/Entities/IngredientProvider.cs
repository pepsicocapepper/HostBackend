using Domain.Common;

namespace Domain.Entities;

public class IngredientProvider
{
    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
}