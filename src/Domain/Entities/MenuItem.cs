namespace Domain.Entities;

public class MenuItem
{
    public int MenuId { get; set; }
    public Menu Menu { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int DisplayOrder { get; set; }
}