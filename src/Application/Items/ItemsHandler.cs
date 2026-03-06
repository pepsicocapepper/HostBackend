using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items.Dtos;
using Domain.Entities;

namespace Application.Items;

public class ItemsHandler : IItemsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;

    public ItemsHandler(IApplicationDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<int> CreateProduct(CreateItemDto createItemDto,
        CancellationToken cancellationToken = default)
    {
        var product = new Item
        {
            Name = createItemDto.Name,
            Price = createItemDto.Price,
            CreatedBy = _userContext.UserId!.Value
        };

        await _dbContext.Item.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

    public Task<PaginatedData<Item>> GetProducts(CancellationToken cancellationToken = default)
    {
        var products = _dbContext
            .Item
            .PaginatedListAsync(1, 10, cancellationToken);
        return products;
    }
}