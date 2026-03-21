using Application.Items.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Menus.Dtos;

public class PosSubgroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PosName { get; set; }
    public int DisplayOrder { get; set; }
    public int? ParentMenuId { get; set; }
    public List<ItemWithPriceDto> Items { get; set; }

    private class PosSubgroupDtoProfile : Profile
    {
        public PosSubgroupDtoProfile()
        {
            CreateMap<Menu, PosSubgroupDto>();
        }
    } 
}