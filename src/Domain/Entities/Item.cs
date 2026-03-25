using Domain.Common;

namespace Domain.Entities;

public class Item : BaseEntity<int>
{
    public required string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<ItemPrice> Prices { get; set; } = new List<ItemPrice>();
}