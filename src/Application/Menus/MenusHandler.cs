using Application.Common.Interfaces;
using Application.Menus.Dtos;
using Application.Products;
using Application.Products.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Menus;

public class MenusHandler : IMenusHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProductsHandler _productsHandler;

    public MenusHandler(IApplicationDbContext dbContext, IMapper mapper, IProductsHandler productsHandler)
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

    public async Task<IEnumerable<MenusResponseDto>> GetMenus(CancellationToken cancellationToken = default)
    {
        var allItems = await _dbContext.Menus.ToListAsync(cancellationToken);
        var tree = allItems.Where(i => i.ParentMenuId == null).ToList();
        return _mapper.Map<IEnumerable<MenusResponseDto>>(tree);
    }

    public async Task<int> CreateItemInMenu(int menuId, CreateProductDto createProductDto,
        CancellationToken cancellationToken = default)
    {
        var productId = await _productsHandler.CreateProduct(createProductDto, cancellationToken);
        
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
}