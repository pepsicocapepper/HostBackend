using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Items;

public class ItemsHandler : IItemsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public ItemsHandler(IApplicationDbContext dbContext, IUserContext userContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<int> CreateItem(CreateItemDto createItemDto,
        CancellationToken cancellationToken = default)
    {
        var product = new Item
        {
            Name = createItemDto.Name,
            CreatedBy = _userContext.UserId!.Value,
            BasePrices = createItemDto.Prices.Select(ip => new ItemBasePrice
            {
                Price = ip.Price,
                Denomination = ip.Denomination
            }).ToList()
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

    public async Task<IEnumerable<ItemWithPriceDto>> GetAllItems(Denomination? denomination,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Item
            .Where(i => i.BasePrices.Any(ip => denomination == null || ip.Denomination == denomination))
            .Include(i => i.BasePrices)
            .ProjectTo<ItemWithPriceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}