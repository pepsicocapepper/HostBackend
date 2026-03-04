using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Pin { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
    public IEnumerable<Product>? CreatedProducts { get; set; }
    public IEnumerable<Product>? UpdatedProducts { get; set; }
}