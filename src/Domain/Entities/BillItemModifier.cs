using Domain.Common;

namespace Domain.Entities;

public class BillItemModifier
{
    public Guid BillItemId { get; set; }
    public BillItem BillItem { get; set; } = null!;
    public Guid ModifierElementId { get; set; }
    public ModifierElement ModifierElement { get; set; } = null!;
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
}