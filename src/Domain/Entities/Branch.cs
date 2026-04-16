using Domain.Common;

namespace Domain.Entities;

public class Branch : BaseEntity<Guid>
{
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? ZipCode { get; set; }
    public required string Locality { get; set; }
    public required string AdministrativeArea { get; set; }
    public required string Country { get; set; }
    public ICollection<User> Users { get; set; } = [];
    public ICollection<BranchIngredient> BranchIngredients { get; set; } = [];
}