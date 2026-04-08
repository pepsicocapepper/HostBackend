using Application.Common.Models;
using Application.Menus.Dtos;
using Application.Items.Dtos;
using Domain.Common;
using Domain.Entities;
using ErrorOr;

namespace Application.Menus;

public interface IMenusHandler
{
    Task<ErrorOr<int>> CreateMenu(CreateMenuDto createMenuDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<MenuDto>> GetMenus(CancellationToken cancellationToken = default);

    Task<IEnumerable<PosMenuDto>>
        GetAllMenus(Denomination? denomination, CancellationToken cancellationToken = default);

    Task<ErrorOr<int>> CreateItemInMenu(int menuId, CreateItemDto createItemDto,
        CancellationToken cancellationToken = default);

    Task<PaginatedData<ItemDto>> GetItemsInMenu(int menuId, CancellationToken cancellationToken = default);
}