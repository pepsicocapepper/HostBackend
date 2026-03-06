using Application.Common.Models;
using Application.Items.Dtos;
using Domain.Entities;

namespace Application.Items;

public interface IItemsHandler
{
    Task<int> CreateProduct(CreateItemDto createItemDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<Item>> GetProducts(CancellationToken cancellationToken);
}