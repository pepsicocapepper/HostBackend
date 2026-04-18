using Api.Common.Extensions;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using Application.Recipes;
using Application.Recipes.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Recipes
{
    public static void MapRecipes(this WebApplication app)
    {
        var group = app.MapGroup("/recipes");
        group.MapGet("/", GetPaginatedRecipes).RequireAuthorization();
        group.MapGet("/{recipeId:guid}/available-ingredients", GetPaginatedIngredientsNotInRecipe)
            .RequireAuthorization();
        group.MapPost("/", CreateRecipe).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<RecipeDto>>> GetPaginatedRecipes(int? pageNumber, int? pageSize,
        [FromServices] IRecipesHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedRecipes(new PaginationQuery(pageNumber, pageSize), ct);

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedData<IngredientDto>>, NotFound<ProblemDetails>>>
        GetPaginatedIngredientsNotInRecipe(int? pageNumber, int? pageSize, Guid recipeId,
            [FromServices] IRecipesHandler handler, CancellationToken ct)
    {
        var result =
            await handler.GetPaginatedIngredientsNotInRecipe(new PaginationQuery(pageNumber, pageSize), recipeId, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Created> CreateRecipe(CreateRecipeDto dto, [FromServices] IRecipesHandler handler,
        CancellationToken cancellationToken = default)
    {
        await handler.CreateRecipe(dto, cancellationToken);

        return TypedResults.Created();
    }
}