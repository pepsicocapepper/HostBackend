using Domain.Common;

namespace Domain.Entities;

public class ItemPrice : BaseEntity<int>
{
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}