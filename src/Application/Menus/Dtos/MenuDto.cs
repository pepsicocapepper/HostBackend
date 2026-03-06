using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Menus.Dtos;

public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PosName { get; set; }
    public int DisplayOrder { get; set; }
    public IEnumerable<MenuDto> SubMenus { get; set; } = new List<MenuDto>();
    public bool IsRootMenu { get; set; }

    private class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.IsRootMenu,
                    opt => opt.MapFrom(src => src.ParentMenuId == null));
        }
    }
}