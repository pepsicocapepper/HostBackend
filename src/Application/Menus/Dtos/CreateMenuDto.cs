namespace Application.Menus.Dtos;

public class CreateMenuDto
{
    public required string Name { get; set; }
    public string? PosName { get; set; }
    public int? ParentId { get; set; }
}