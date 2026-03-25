using Domain.Common;

namespace Domain.Entities;

public class ItemSizePrice : BaseEntity<int>
{
    public required string Size { get; set; }
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}