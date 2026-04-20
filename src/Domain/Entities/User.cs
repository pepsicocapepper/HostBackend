using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Pin { get; set; }
    public required string JobTitle  { get; set; }
    public string? Phone  { get; set; }
    public bool? Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
    public IEnumerable<Item>? CreatedItems { get; set; }
    public IEnumerable<Item>? UpdatedItems { get; set; }
    public IEnumerable<Bill>? Bills { get; set; }
    public IEnumerable<PunchingTime>? PunchingTimes { get; set; }
    public required Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
}