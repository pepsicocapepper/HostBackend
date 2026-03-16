using Application.Items.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Menus.Dtos;

public class RawMenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PosName { get; set; }
    public int DisplayOrder { get; set; }
    public int? ParentMenuId { get; set; }
    public List<ItemDto> Items { get; set; }

    private class RawMenuProfile : Profile
    {
        public RawMenuProfile()
        {
            CreateMap<Menu, RawMenuDto>();
        }
    }
}