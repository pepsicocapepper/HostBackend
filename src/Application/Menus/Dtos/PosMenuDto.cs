using System.Linq.Expressions;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Menus.Dtos;

public class PosMenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PosName { get; set; }
    public int? Color { get; set; }
    public int DisplayOrder { get; set; }
    public IEnumerable<PosSubgroupDto>? Subgroups { get; set; }

    private class PosMenuProfile : Profile
    {
        public PosMenuProfile()
        {
            CreateMap<Menu, PosMenuDto>()
                .ForMember(d => d.Subgroups,
                    opt => opt.MapFrom(m => m.SubMenus)
                );
        }
    }
}