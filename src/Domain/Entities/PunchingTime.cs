using Domain.Common;

namespace Domain.Entities;

public class PunchingTime : BaseEntity<Guid>
{
    public required char InOrOut { get; set; }
    public bool Active { get; set; }
    public required Guid UserId { get; set; }
    public  User User { get; set; } = null!;
    public  DateTime CreatedAt { get; set; }
}