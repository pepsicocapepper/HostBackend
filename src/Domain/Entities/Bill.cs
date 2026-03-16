using Domain.Common;

namespace Domain.Entities;

public class Bill : BaseEntity<Guid>
{
    public required decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();
}