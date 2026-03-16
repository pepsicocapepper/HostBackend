using Application.Common.Models;
using Application.Items.Dtos;
using Domain.Entities;

namespace Application.Items;

public interface IItemsHandler
{
    Task<int> CreateItem(CreateItemDto createItemDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<Item>> GetPaginatedItems(CancellationToken cancellationToken);
    Task<IEnumerable<ItemDto>> GetAllItems(CancellationToken cancellationToken = default);
}