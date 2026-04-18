using Application.Common.Models;
using Application.Ingredients.Dtos;
using Application.Recipes.Dtos;
using ErrorOr;

namespace Application.Recipes;

public interface IRecipesHandler
{
    public Task<PaginatedData<RecipeDto>> GetPaginatedRecipes(PaginationQuery query,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<PaginatedData<IngredientDto>>> GetPaginatedIngredientsNotInRecipe(PaginationQuery query,
        Guid recipeId,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<int>> CreateRecipe(CreateRecipeDto dto, CancellationToken cancellationToken = default);
}