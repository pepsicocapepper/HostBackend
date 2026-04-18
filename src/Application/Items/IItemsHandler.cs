using Application.Common.Models;
using Application.Items.Dtos;
using Application.Recipes.Dtos;
using Domain.Common;
using Domain.Entities;
using ErrorOr;

namespace Application.Items;

public interface IItemsHandler
{
    Task<ErrorOr<int>> CreateItem(CreateItemDto createItemDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<ItemDto>> GetPaginatedItems(PaginationQuery query, CancellationToken cancellationToken);

    Task<ErrorOr<PaginatedData<RecipeDto>>> GetPaginatedRecipesNotInItem(PaginationQuery query,
        int itemId, CancellationToken cancellationToken = default);

    Task<IEnumerable<ItemWithPriceDto>> GetAllItems(Denomination? denomination,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ItemIngredientDto>> GetAllIngredients(int itemId, CancellationToken cancellationToken = default);
    Task DeleteItem(int itemId, CancellationToken cancellationToken = default);
}