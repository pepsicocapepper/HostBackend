namespace Domain.Entities;

public class ModifierGroupElement
{
    public Guid ModifierGroupId { get; set; }
    public ModifierGroup ModifierGroup { get; set; } = null!;
    public Guid ModifierElementId { get; set; }
    public ModifierElement ModifierElement { get; set; } = null!;
}