using Application.Menus;
using Application.Menus.Dtos;
using Application.Products.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Menus
{
    public static void MapMenus(this WebApplication app)
    {
        var group = app.MapGroup("/menus");

        group.MapGet("/", GetMenus).RequireAuthorization();
        group.MapPost("/", CreateMenu).RequireAuthorization();
        group.MapPost("/{menuId}/items", CreateItemInMenu).RequireAuthorization();
    }

    private static async Task<Created<int>> CreateMenu([FromBody] CreateMenuDto createMenuDto,
        [FromServices] IMenusHandler handler, CancellationToken ct)
    {
        var result = await handler.CreateMenu(createMenuDto, ct);
        return TypedResults.Created("/menus", result);
    }

    private static async Task<Ok<IEnumerable<MenusResponseDto>>> GetMenus([FromServices] IMenusHandler handler,
        CancellationToken ct)
    {
        var menus = await handler.GetMenus(ct);
        return TypedResults.Ok(menus);
    }

    private static async Task<Ok<int>> CreateItemInMenu([FromBody] CreateProductDto createProductDto,
        [FromServices] IMenusHandler handler, CancellationToken ct, string menuId)
    {
        var parsedMenuId = int.Parse(menuId);
        var result = await handler.CreateItemInMenu(parsedMenuId, createProductDto, ct);
        return TypedResults.Ok(result);
    }
}