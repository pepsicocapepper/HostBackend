using Application.Menus.Dtos;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Menus;

public interface IMenusHandler
{
    Task<int> CreateMenu(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenusResponseDto>> GetMenus(CancellationToken cancellationToken = default);

    Task<int> CreateItemInMenu(int menuId, CreateProductDto createProductDto,
        CancellationToken cancellationToken = default);
}