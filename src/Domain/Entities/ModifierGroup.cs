using Domain.Common;

namespace Domain.Entities;

public class ModifierGroup : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public ICollection<ModifierGroupElement> ModifierGroupElements { get; set; } = [];
    public ICollection<ItemModifierGroup> ItemModifierGroups { get; set; } = [];
}