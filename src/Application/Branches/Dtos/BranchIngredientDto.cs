using AutoMapper;
using Domain.Entities;

namespace Application.Branches.Dtos;

public class BranchIngredientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = null!;

    private class BranchIngredientDtoProfile : Profile
    {
        public BranchIngredientDtoProfile()
        {
            CreateMap<BranchIngredient, BranchIngredientDto>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Ingredient.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(t => t.Ingredient.Name));
        }
    }
}