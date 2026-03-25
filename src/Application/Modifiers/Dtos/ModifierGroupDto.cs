using AutoMapper;
using Domain.Entities;

namespace Application.Modifiers.Dtos;

public class ModifierGroupDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<ModifierElementDto> ModifierElements { get; set; } = [];

    private class ModifierGroupProfile : Profile
    {
        public ModifierGroupProfile()
        {
            CreateMap<ModifierGroup, ModifierGroupDto>();
        }
    }
}