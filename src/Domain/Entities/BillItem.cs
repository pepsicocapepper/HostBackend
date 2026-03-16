namespace Domain.Entities;

public class BillItem
{
    public Guid BillId { get; set; }
    public int ItemId { get; set; }
    public Bill Bill { get; set; } = null!;
    public Item Item { get; set; } = null!;
    public int Quantity { get; set; }
}