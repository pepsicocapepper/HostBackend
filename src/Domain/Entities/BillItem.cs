using Domain.Common;

namespace Domain.Entities;

public class BillItem : BaseEntity<Guid>
{
    public Guid BillId { get; set; }
    public int ItemId { get; set; }
    public Bill Bill { get; set; } = null!;
    public Item Item { get; set; } = null!;
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
    public int Quantity { get; set; }

    public ICollection<BillItemModifier> BillItemModifiers = [];
}