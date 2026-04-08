using Application.Common.Interfaces;
using Application.Ingredients.Dtos;
using Domain.Entities;

namespace Application.Ingredients;

internal class IngredientsHandler : IIngredientsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public IngredientsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateIngredientAsync(CreateIngredientDto dto, CancellationToken cancellationToken = default)
    {
        var ingredient = new Ingredient
        {
            Name = dto.Name,
        };

        await _dbContext.Ingredients.AddAsync(ingredient, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ingredient.Id;
    }
}