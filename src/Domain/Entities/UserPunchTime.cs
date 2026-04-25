using Domain.Common;

namespace Domain.Entities;

public class UserPunchTime : BaseEntity<int>
{
    public bool IsEntrance { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}