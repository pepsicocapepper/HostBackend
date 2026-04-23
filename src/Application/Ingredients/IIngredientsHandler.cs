using Application.Common.Models;
using Application.Ingredients.Dtos;
using ErrorOr;

namespace Application.Ingredients;

public interface IIngredientsHandler
{
    public Task<PaginatedData<IngredientDto>> GetPaginatedIngredients(CancellationToken cancellationToken = default);

    public Task<ErrorOr<int>> CreateIngredient(CreateIngredientDto dto,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<bool>> UpdateIngredient(UpdateIngredientDto dto, CancellationToken cancellationToken = default);
}