using Application.Ingredients.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Providers.Dtos;

public class IngredientProviderDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    private class IngredientProviderDtoProfile : Profile
    {
        public IngredientProviderDtoProfile()
        {
            CreateMap<IngredientProvider, IngredientProviderDto>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => t.Ingredient.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(t => t.Ingredient.Name));
        }
    }
}