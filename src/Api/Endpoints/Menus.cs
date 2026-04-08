using Api.Common.Extensions;
using Application.Common.Models;
using Application.Items.Dtos;
using Application.Menus;
using Application.Menus.Dtos;
using Domain.Common.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Menus
{
    public static void MapMenus(this WebApplication app)
    {
        var group = app.MapGroup("/menus");

        group.MapGet("/", GetMenus);
        group.MapGet("/all", GetAllMenus).RequireHeader("X-Pos-Key");
        group.MapPost("/", CreateMenu).RequireAuthorization();
        group.MapGet("/{menuId}/items", GetItemsInMenu).RequireAuthorization();
        group.MapPost("/{menuId}/items", CreateItemInMenu).RequireAuthorization();
    }

    private static async Task<Results<Created<int>, BadRequest<ProblemDetails>>> CreateMenu(
        [FromBody] CreateMenuDto createMenuDto,
        [FromServices] IMenusHandler handler,
        CancellationToken ct)
    {
        var result = await handler.CreateMenu(createMenuDto, ct);
        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created("/menus", result.Value);
    }

    private static async Task<Ok<IEnumerable<MenuDto>>> GetMenus(
        [FromServices] IMenusHandler handler,
        CancellationToken ct)
    {
        var menus = await handler.GetMenus(ct);
        return TypedResults.Ok(menus);
    }

    private static async Task<Ok<IEnumerable<PosMenuDto>>> GetAllMenus(
        [FromServices] IMenusHandler handler,
        [FromQuery] string denomination,
        CancellationToken ct
    )
    {
        var menus = await handler.GetAllMenus(denomination.TryToDenomination(), ct);
        return TypedResults.Ok(menus);
    }

    private static async Task<Results<Ok<int>, BadRequest<ProblemDetails>>> CreateItemInMenu(
        [FromBody] CreateItemDto createProductDto,
        [FromServices] IMenusHandler handler,
        CancellationToken ct, string menuId)
    {
        var parsedMenuId = int.Parse(menuId);
        var result = await handler.CreateItemInMenu(parsedMenuId, createProductDto, ct);
        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<PaginatedData<ItemDto>>> GetItemsInMenu([FromServices] IMenusHandler handler,
        CancellationToken ct, string menuId)
    {
        var parsedMenuId = int.Parse(menuId);
        var result = await handler.GetItemsInMenu(parsedMenuId, ct);
        return TypedResults.Ok(result);
    }
}