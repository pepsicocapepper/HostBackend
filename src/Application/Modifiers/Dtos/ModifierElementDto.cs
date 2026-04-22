using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Modifiers.Dtos;

public class ModifierElementDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Denomination { get; set; }

    private class ModifierElementDtoProfile : Profile
    {
        public ModifierElementDtoProfile()
        {
            CreateMap<ModifierElement, ModifierElementDto>();
        }
    }
}