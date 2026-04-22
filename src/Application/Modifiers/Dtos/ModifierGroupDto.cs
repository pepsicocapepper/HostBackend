using AutoMapper;
using Domain.Entities;

namespace Application.Modifiers.Dtos;

public class ModifierGroupDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    private class ModifierGroupsDtoProfile : Profile
    {
        public ModifierGroupsDtoProfile()
        {
            CreateMap<ModifierGroup, ModifierGroupDto>();
        }
    }
}