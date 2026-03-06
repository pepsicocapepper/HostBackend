namespace Application.Menus.Dtos;

public class MenusResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PosName {get; set;}
    public int DisplayOrder { get; set; }
    public IEnumerable<MenusResponseDto> SubMenus { get; set; } = new List<MenusResponseDto>();
}