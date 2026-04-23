using Api.Common.Extensions;
using Application.Common.Models;
using Application.Ingredients;
using Application.Ingredients.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Ingredients
{
    public static void MapIngredients(this WebApplication app)
    {
        var group = app.MapGroup("/ingredients");
        group.MapGet("/", GetPaginatedIngredients).RequireAuthorization();
        group.MapPost("/", CreateIngredient).RequireAuthorization();
        group.MapPatch("/", UpdateIngredient).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<IngredientDto>>> GetPaginatedIngredients(
        [FromServices] IIngredientsHandler handler, CancellationToken ct)
    {
        return TypedResults.Ok(await handler.GetPaginatedIngredients(ct));
    }

    private static async Task<Results<Created, BadRequest<ProblemDetails>>> CreateIngredient(
        [FromBody] CreateIngredientDto dto,
        [FromServices] IIngredientsHandler handler, CancellationToken ct)
    {
        var result = await handler.CreateIngredient(dto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }


    private static async Task<Results<Ok, BadRequest<ProblemDetails>>> UpdateIngredient(
        [FromBody] UpdateIngredientDto dto,
        [FromServices] IIngredientsHandler handler, CancellationToken ct)
    {
        var result = await handler.UpdateIngredient(dto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok();
    }
}