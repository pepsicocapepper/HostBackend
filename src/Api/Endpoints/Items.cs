using Application.Common.Models;
using Application.Items;
using Application.Items.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Items
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/items");

        group.MapGet("/", GetPaginatedProducts).WithName("GetProducts");
        group.MapPost("/", CreateProduct).RequireAuthorization();
    }

    private static async Task<CreatedAtRoute<int>> CreateProduct([FromServices] IItemsHandler handler,
        [FromBody] CreateItemDto dto,
        CancellationToken ct)
    {
        var id = await handler.CreateProduct(dto, ct);
        return TypedResults.CreatedAtRoute(id, "GetProducts", new { });
    }

    private static async Task<Ok<PaginatedData<Item>>> GetPaginatedProducts([FromServices] IItemsHandler handler,
        CancellationToken ct)
    {
        var products = await handler.GetProducts(ct);
        return TypedResults.Ok(products);
    }
}