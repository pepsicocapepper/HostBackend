using AutoMapper;
using Domain.Entities;

namespace Application.Recipes.Dtos;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Steps { get; set; } = [];
    public int IngredientsCount { get; set; }

    private class RecipeDtoProfile : Profile
    {
        public RecipeDtoProfile()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(d => d.IngredientsCount, opts => opts.MapFrom(src => src.RecipeIngredients.Count));
        }
    }
}