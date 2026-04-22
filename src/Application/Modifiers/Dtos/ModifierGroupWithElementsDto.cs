using AutoMapper;
using Domain.Entities;

namespace Application.Modifiers.Dtos;

public class ModifierGroupWithElementsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<ModifierElementDto> ModifierElements { get; set; } = [];

    private class ModifierGroupWithElementsDtoProfile : Profile
    {
        public ModifierGroupWithElementsDtoProfile()
        {
            CreateMap<ModifierGroup, ModifierGroupWithElementsDto>();
        }
    }
}