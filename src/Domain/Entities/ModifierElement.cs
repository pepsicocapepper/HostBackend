using Domain.Common;

namespace Domain.Entities;

public class ModifierElement : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
    public ICollection<ModifierGroupElement> ModifierGroupElements { get; set; } = [];
    public ICollection<BillItemModifier> BillItemModifiers { get; set; } = [];
}