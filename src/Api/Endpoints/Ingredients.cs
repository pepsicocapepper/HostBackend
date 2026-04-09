using Api.Common.Extensions;
using Application.Common.Models;
using Application.Ingredients;
using Application.Ingredients.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Ingredients
{
    const string PaginatedIngredients = "PaginatedIngredients";

    public static void MapIngredients(this WebApplication app)
    {
        var group = app.MapGroup("/ingredients");
        group.MapGet("/", GetPaginatedIngredients).RequireAuthorization();
        group.MapPost("/", CreateIngredient).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<IngredientDto>>> GetPaginatedIngredients(
        [FromServices] IIngredientsHandler handler, CancellationToken ct)
    {
        return TypedResults.Ok(await handler.GetPaginatedIngredients(ct));
    }

    private static async Task<Results<CreatedAtRoute<int>, BadRequest<ProblemDetails>>> CreateIngredient(
        [FromBody] CreateIngredientDto dto,
        [FromServices] IIngredientsHandler handler, CancellationToken ct)
    {
        var result = await handler.CreateIngredient(dto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.CreatedAtRoute(result.Value, PaginatedIngredients, new { });
    }
}