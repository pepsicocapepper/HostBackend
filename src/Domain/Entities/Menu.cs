using Domain.Common;

namespace Domain.Entities;

public class Menu : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? PosName { get; set; }
    public int DisplayOrder { get; set; }
    public int? ParentMenuId { get; set; }
    public virtual Menu? ParentMenu { get; set; }
    public virtual ICollection<Menu>? SubMenus { get; set; } = new List<Menu>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); 
}