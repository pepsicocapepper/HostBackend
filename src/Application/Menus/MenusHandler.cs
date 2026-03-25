using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Items;
using Application.Menus.Dtos;
using Application.Items.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Menus;

public class MenusHandler : IMenusHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IItemsHandler _productsHandler;

    public MenusHandler(IApplicationDbContext dbContext, IMapper mapper, IItemsHandler productsHandler)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _productsHandler = productsHandler;
    }

    public async Task<int> CreateMenu(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default)
    {
        var menu = new Menu
        {
            Name = createMenuDto.Name,
            PosName = createMenuDto.PosName,
            ParentMenuId = createMenuDto.ParentId
        };

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

    public async Task<IEnumerable<PosMenuDto>> GetAllMenus(CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Menus
            .Include(m => m.MenuItems)
            .Where(m => m.ParentMenuId == null)
            .Select(menu => new PosMenuDto
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    PosName = menu.PosName,
                    Subgroups = menu.SubMenus != null
                        ? menu.SubMenus.Select(sg =>
                            new PosSubgroupDto
                            {
                                Id = sg.Id,
                                Name = sg.Name,
                                PosName = sg.PosName,
                                Items = sg.MenuItems.Select(mi => new ItemWithPriceDto
                                    {
                                        Id = mi.Item.Id,
                                        Name = mi.Item.Name,
                                        Price = mi.Item.Prices
                                            .Select(p => new ItemPriceDto
                                            {
                                                Price = p.Price,
                                                Denomination = p.Denomination.ToString()
                                            }).First(),
                                        CreatedAt = mi.Item.CreatedAt,
                                        CreatedBy = mi.Item.CreatedBy,
                                        UpdatedAt = mi.Item.UpdatedAt,
                                        UpdatedBy = mi.Item.UpdatedBy
                                    }
                                ).ToList(),
                                DisplayOrder = sg.DisplayOrder,
                                ParentMenuId = sg.ParentMenuId
                            })
                        : null
                }
            ).ToListAsync(cancellationToken);
    }

    public async Task<int> CreateItemInMenu(int menuId, CreateItemDto createItemDto,
        CancellationToken cancellationToken = default)
    {
        var productId = await _productsHandler.CreateItem(createItemDto, cancellationToken);

        var menuExists = await _dbContext.Menus.AnyAsync(m => m.Id == menuId, cancellationToken);
        if (!menuExists)
        {
            throw new ApplicationException($"Menu with id {menuId} not found");
        }

        await _dbContext.MenuItems.AddAsync(new MenuItem
        {
            ItemId = productId,
            MenuId = menuId,
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return productId;
    }

    public async Task<PaginatedData<ItemDto>> GetItemsInMenu(int menuId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MenuItems.Where(mi => mi.MenuId == menuId).Select(mi => mi.Item)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider, cancellationToken)
            .PaginatedListAsync(1, 10, cancellationToken: cancellationToken);
    }
}