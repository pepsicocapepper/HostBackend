using Domain.Common;

namespace Domain.Entities;

public class UserPunchTime : BaseEntity<int>
{
    public required bool IsEntrance { get; set; }
    public required Guid UserId { get; set; }
    public  User User { get; set; } = null!;
    public  DateTime CreatedAt { get; set; }
}