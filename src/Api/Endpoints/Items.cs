using Api.Common.Extensions;
using Application.Common.Models;
using Application.Items;
using Application.Items.Dtos;
using Application.Recipes.Dtos;
using Domain.Common.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Items
{
    const string PaginatedItems = "PaginatedItems";
    const string AllItems = "AllItems";

    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/items");

        group.MapGet("/", GetPaginatedItems).WithName(PaginatedItems).RequireAuthorization();
        group.MapGet("/all", GetAllItems).WithName(AllItems);
        group.MapPost("/", CreateProduct).RequireAuthorization();
        group.MapDelete("/{itemId:int}", DeleteItem).RequireAuthorization();
        group.MapGet("/{itemId:int}/ingredients", GetIngredients).RequireAuthorization();
        group.MapGet("/{itemId:int}/available-recipes", GetPaginatedRecipesNotInItem).RequireAuthorization();
    }

    private static async Task<Ok<IEnumerable<ItemIngredientDto>>> GetIngredients(int itemId,
        [FromServices] IItemsHandler handler,
        CancellationToken ct)
    {
        return TypedResults.Ok(await handler.GetAllIngredients(itemId, ct));
    }

    private static async Task<Results<CreatedAtRoute<int>, BadRequest<ProblemDetails>>> CreateProduct(
        [FromServices] IItemsHandler handler,
        [FromBody] CreateItemDto dto,
        CancellationToken ct)
    {
        var result = await handler.CreateItem(dto, ct);
        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.CreatedAtRoute(result.Value, PaginatedItems, new { });
    }

    private static async Task<Ok<PaginatedData<ItemDto>>> GetPaginatedItems(int? pageNumber, int? pageSize,
        [FromServices] IItemsHandler handler, CancellationToken ct)
    {
        var products = await handler.GetPaginatedItems(new PaginationQuery(pageNumber, pageSize), ct);
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

    private static async Task<Results<Ok<PaginatedData<RecipeDto>>, NotFound<ProblemDetails>>>
        GetPaginatedRecipesNotInItem(int? pageNumber, int? pageSize, int itemId, [FromServices] IItemsHandler handler,
            CancellationToken ct)
    {
        var result = await handler.GetPaginatedRecipesNotInItem(new PaginationQuery(pageNumber, pageSize), itemId, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok> DeleteItem(
        int itemId,
        [FromServices] IItemsHandler handler,
        CancellationToken ct
    )
    {
        await handler.DeleteItem(itemId, ct);
        return TypedResults.Ok();
    }
}