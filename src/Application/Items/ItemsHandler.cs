using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items.Dtos;
using Application.Recipes.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Items;

public class ItemsHandler : IItemsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateItemDto> _createItemDtoValidator;

    public ItemsHandler(IApplicationDbContext dbContext, IUserContext userContext, IMapper mapper,
        IValidator<CreateItemDto> validator)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _mapper = mapper;
        _createItemDtoValidator = validator;
    }

    public async Task<ErrorOr<int>> CreateItem(CreateItemDto createItemDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _createItemDtoValidator.Validate(createItemDto);

        if (!validationResult.IsValid)
        {
            return Error.Validation(ItemErrorCodes.ValidationError);
        }

        var item = new Item
        {
            Name = createItemDto.Name,
            PosName = createItemDto.PosName,
            Plu = createItemDto.Plu,
            Sku = createItemDto.Sku,
            Description = createItemDto.Description,
            PricingModel = createItemDto.PricingModel,
            CreatedBy = _userContext.UserId!.Value,
            CreatedAt = DateTime.UtcNow,
            BasePrices = createItemDto.Prices.Select(ip => new ItemBasePrice
            {
                Price = ip.Price,
                Denomination = ip.Denomination
            }).ToList(),
            ItemRecipes = createItemDto.RecipeIds.Select(id => new ItemRecipe
            {
                RecipeId = id
            }).ToList()
        };

        await _dbContext.Items.AddAsync(item, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return item.Id;
    }

    public async Task<PaginatedData<ItemDto>> GetPaginatedItems(PaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Items
            .Where(i => i.IsActive)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<ErrorOr<PaginatedData<RecipeDto>>> GetPaginatedRecipesNotInItem(PaginationQuery query, int itemId,
        CancellationToken cancellationToken = default)
    {
        var itemExists = await _dbContext.Items.AnyAsync(i => i.Id == itemId, cancellationToken);

        if (!itemExists)
        {
            return Error.NotFound(ItemErrorCodes.NotFound);
        }

        return await _dbContext
            .Recipes
            .Where(t => !_dbContext.ItemRecipes.Any(ri => ri.ItemId == itemId && ri.RecipeId == t.Id))
            .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<ItemWithPriceDto>> GetAllItems(Denomination? denomination,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Items
            .Where(i => i.BasePrices.Any(ip => denomination == null || ip.Denomination == denomination) && i.IsActive)
            .Include(i => i.BasePrices)
            .ProjectTo<ItemWithPriceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ItemIngredientDto>> GetAllIngredients(int itemId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .ItemIngredients
            .Where(i => i.ItemId == itemId).Select(i => new ItemIngredientDto
            {
                Name = i.Ingredient.Name,
                Quantity = i.Quantity
            })
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteItem(int itemId, CancellationToken cancellationToken = default)
    {
        var item = await _dbContext
            .Items
            .FindAsync([itemId], cancellationToken);

        if (item is not null)
        {
            item.IsActive = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}