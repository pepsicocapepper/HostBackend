using Domain.Common;

namespace Domain.Entities;

public class Provider : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public ICollection<IngredientProvider> ProviderIngredients { get; set; } = [];
}