using Application.Ingredients.Dtos;
using Domain.Entities;

namespace Application.Ingredients;

public interface IIngredientsHandler
{
    public Task<int> CreateIngredientAsync(CreateIngredientDto ingredient,
        CancellationToken cancellationToken = default);
}