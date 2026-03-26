using Application.Common.Models;
using Application.Items.Dtos;
using Domain.Common;
using Domain.Entities;

namespace Application.Items;

public interface IItemsHandler
{
    Task<int> CreateItem(CreateItemDto createItemDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<ItemDto>> GetPaginatedItems(CancellationToken cancellationToken);

    Task<IEnumerable<ItemWithPriceDto>> GetAllItems(Denomination? denomination,
        CancellationToken cancellationToken = default);
}