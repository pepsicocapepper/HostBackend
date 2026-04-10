using AutoMapper;
using Domain.Entities;

namespace Application.Ingredients.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    private class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDto>();
        }
    }
}