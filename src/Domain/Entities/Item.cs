using Domain.Common;

namespace Domain.Entities;

public class Item : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? PosName { get; set; }
    public string? Sku { get; set; }
    public int? Plu { get; set; }
    public string? Description { get; set; }
    public byte[]? Color { get; set; }
    public PricingModel PricingModel { get; set; }
    public Guid CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public ICollection<ItemBasePrice> BasePrices { get; set; } = [];
    public ICollection<ItemSizePrice> SizePrices { get; set; } = [];
    public ICollection<ItemModifierGroup> ItemModifierGroups { get; set; } = [];
    public ICollection<ItemIngredient> ItemIngredients { get; set; } = [];
}