using Application.Common.Models;
using Application.Items;
using Application.Items.Dtos;
using Domain.Common;
using Domain.Common.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Items
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/items");

        group.MapGet("/", GetPaginatedItems).WithName("GetProducts").RequireAuthorization();
        group.MapGet("/all", GetAllItems).WithName("GetAllItems");
        group.MapPost("/", CreateProduct).RequireAuthorization();
    }

    private static async Task<CreatedAtRoute<int>> CreateProduct(
        [FromServices] IItemsHandler handler,
        [FromBody] CreateItemDto dto,
        CancellationToken ct)
    {
        var id = await handler.CreateItem(dto, ct);
        return TypedResults.CreatedAtRoute(id, "GetProducts", new { });
    }

    private static async Task<Ok<PaginatedData<ItemDto>>> GetPaginatedItems(
        [FromServices] IItemsHandler handler,
        CancellationToken ct)
    {
        var products = await handler.GetPaginatedItems(ct);
        return TypedResults.Ok(products);
    }

    private static async Task<Ok<IEnumerable<ItemWithPriceDto>>> GetAllItems(
        [FromServices] IItemsHandler handler,
        [FromQuery] string denomination,
        CancellationToken ct)
    {
        var items = await handler.GetAllItems(
            denomination.TryToDenomination(),
            ct
        );
        return TypedResults.Ok(items);
    }
}