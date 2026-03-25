using Domain.Common;

namespace Application.Modifiers.Dtos;

public class ModifierElementDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Denomination { get; set; }
}