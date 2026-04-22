using Domain.Common;

namespace Domain.Entities;

public class Staffing : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public ICollection<User> Users { get; set; } = [];
}