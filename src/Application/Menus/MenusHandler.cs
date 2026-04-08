using System.Buffers.Binary;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items;
using Application.Menus.Dtos;
using Application.Items.Dtos;
using Application.Menus.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Menus;

public class MenusHandler : IMenusHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IItemsHandler _productsHandler;
    private readonly IValidator<CreateMenuDto> _createMenuValidator;

    public MenusHandler(IApplicationDbContext dbContext, IMapper mapper, IItemsHandler productsHandler,
        IValidator<CreateMenuDto> createMenuValidator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _productsHandler = productsHandler;
        _createMenuValidator = createMenuValidator;
    }

    public async Task<ErrorOr<int>> CreateMenu(CreateMenuDto createMenuDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _createMenuValidator.Validate(createMenuDto);

        if (!validationResult.IsValid)
        {
            return Error.Validation(MenuErrorCodes.ValidationError, "ValidationError");
        }

        var menu = new Menu
        {
            Name = createMenuDto.Name,
            PosName = createMenuDto.PosName,
            ParentMenuId = createMenuDto.ParentId
        };

        var exists = _dbContext.Menus.Any(m => m.Name == menu.Name);
        if (exists)
        {
            return Error.Conflict(MenuErrorCodes.AlreadyExists,
                description: $"Menu {menu.Name} already exists");
        }

        await _dbContext.Menus.AddAsync(menu, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return menu.Id;
    }

    public async Task<IEnumerable<MenuDto>> GetMenus(CancellationToken cancellationToken = default)
    {
        var allItems = await _dbContext.Menus.ToListAsync(cancellationToken);
        var tree = allItems.Where(i => i.ParentMenuId == null).ToList();
        return _mapper.Map<IEnumerable<MenuDto>>(tree);
    }

    public async Task<IEnumerable<PosMenuDto>> GetAllMenus(Denomination? denomination,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Menus
            .Include(m => m.MenuItems)
            .AsNoTracking()
            .Where(m => m.ParentMenuId == null)
            .Select(menu => new PosMenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    PosName = menu.PosName,
                    Color = menu.Color != null && menu.Color.Length > 0
                        ? BinaryPrimitives.ReadInt32BigEndian(menu.Color)
                        : null,
                    Subgroups = menu.SubMenus != null
                        ? menu.SubMenus.Select(sg =>
                            new PosSubgroupDto
                            {
                                Id = sg.Id,
                                Name = sg.Name,
                                PosName = sg.PosName,
                                Color = sg.Color != null && sg.Color.Length > 0
                                    ? BinaryPrimitives.ReadInt32BigEndian(sg.Color)
                                    : null,
                                Items = sg.MenuItems
                                    .AsQueryable()
                                    .Where(mi =>
                                        mi.Item.PricingModel == PricingModel.Base
                                            ? mi.Item.BasePrices.Any(ip =>
                                                denomination == null || ip.Denomination == denomination)
                                            : mi.Item.SizePrices.Any(ip =>
                                                denomination == null || ip.Denomination == denomination)
                                    )
                                    .Select(
                                        ItemMappings.ToItemWithPriceDto(denomination)
                                    ).ToList(),
                                DisplayOrder = sg.DisplayOrder,
                                ParentMenuId = sg.ParentMenuId
                            })
                        : null
                }
            ).ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<int>> CreateItemInMenu(
        int menuId,
        CreateItemDto createItemDto,
        CancellationToken cancellationToken = default)
    {
        var productId = await _productsHandler.CreateItem(createItemDto, cancellationToken);
        if (productId.IsError)
        {
            return productId.Errors;
        }

        var menuExists = await _dbContext.Menus.AnyAsync(m => m.Id == menuId, cancellationToken);
        if (!menuExists)
        {
            return Error.NotFound(MenuErrorCodes.NotFound);
        }

        await _dbContext.MenuItems.AddAsync(new MenuItem
        {
            ItemId = productId.Value,
            MenuId = menuId,
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return productId;
    }

    public async Task<PaginatedData<ItemDto>> GetItemsInMenu(int menuId, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .MenuItems
            .Where(mi => mi.MenuId == menuId)
            .Select(mi => mi.Item)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider, cancellationToken)
            .PaginatedListAsync(1, 10, cancellationToken: cancellationToken);
    }
}