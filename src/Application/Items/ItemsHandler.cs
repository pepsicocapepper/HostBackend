using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Items;

public class ItemsHandler : IItemsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public ItemsHandler(IApplicationDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<int> CreateItem(CreateItemDto createItemDto,
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

    public Task<PaginatedData<Item>> GetPaginatedItems(CancellationToken cancellationToken = default)
    {
        var products = _dbContext
            .Item
            .PaginatedListAsync(1, 10, cancellationToken);
        return products;
    }

    public async Task<IEnumerable<ItemDto>> GetAllItems(CancellationToken cancellationToken = default)
    {
        var allItems = await _dbContext
            .Item.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ItemDto>>(allItems);
    }
}