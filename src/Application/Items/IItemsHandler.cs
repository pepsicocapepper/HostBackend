using Application.Common.Models;
using Application.Items.Dtos;
using Domain.Common;
using Domain.Entities;
using ErrorOr;

namespace Application.Items;

public interface IItemsHandler
{
    Task<ErrorOr<int>> CreateItem(CreateItemDto createItemDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<ItemDto>> GetPaginatedItems(CancellationToken cancellationToken);

    Task<IEnumerable<ItemWithPriceDto>> GetAllItems(Denomination? denomination,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ItemIngredientDto>> GetAllIngredients(int itemId, CancellationToken cancellationToken = default);
    Task DeleteItem(int itemId, CancellationToken cancellationToken = default);
}