using Domain.Common;

namespace Domain.Entities;

public class RefreshToken : BaseEntity<int>
{
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public bool Invalidated { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}