using Domain.Common;

namespace Domain.Entities;

public class ItemModifierGroup : BaseEntity<int>
{
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public Guid ModifierGroupId { get; set; }
    public ModifierGroup ModifierGroups { get; set; } = null!;
    public int DisplayOrder { get; set; }
}