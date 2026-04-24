using Api.Common.Extensions;
using Application.Common.Models;
using Application.Providers;
using Application.Providers.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Providers
{
    const string PaginatedProviders = "PaginatedProviders";

    public static void MapProviders(this WebApplication app)
    {
        var group = app.MapGroup("/providers");
        group.MapGet("/", GetPaginatedProviders).RequireAuthorization().WithName(PaginatedProviders);
        group.MapGet("/{id}", GetProviderById).RequireAuthorization();
        group.MapGet("/{providerId:guid}/ingredients", GetPaginatedProviderIngredients).RequireAuthorization();
        group.MapPost("/", CreateProvider).RequireAuthorization();
        group.MapPatch("/", UpdateProvider).RequireAuthorization();
        group.MapPost("/{providerId:guid}/ingredients/${ingredientId:int}", AddIngredient).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<ProviderDto>>> GetPaginatedProviders(
        int? pageSize,
        int? pageNumber,
        [FromServices] IProvidersHandler handler,
        CancellationToken ct
    )
    {
        var result = await handler.GetPaginatedProviders(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<ProviderDto>, NotFound<ProblemDetails>>> GetProviderById(
        string id, [FromServices] IProvidersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetProviderById(new Guid(id), ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<PaginatedData<IngredientProviderDto>>> GetPaginatedProviderIngredients(
        int? pageSize,
        int? pageNumber,
        Guid providerId,
        [FromServices] IProvidersHandler handler,
        CancellationToken ct
    )
    {
        var result =
            await handler.GetPaginatedProviderIngredients(new PaginationQuery(pageNumber, pageSize), providerId, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<CreatedAtRoute<Guid>, BadRequest<ProblemDetails>>> CreateProvider(
        [FromBody] CreateProviderDto dto,
        [FromServices] IProvidersHandler handler,
        CancellationToken ct)
    {
        var result = await handler.CreateProvider(dto, ct);
        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.CreatedAtRoute(result.Value, PaginatedProviders, new { });
    }
    
    private static async Task<Results<Ok, NotFound<ProblemDetails>>> UpdateProvider(
        [FromBody] UpdateProviderDto dto,
        [FromServices] IProvidersHandler handler,
        CancellationToken ct)
    {
        var result = await handler.UpdateProvider(dto, ct);
        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok();
    }

    private static async Task<Results<Created, NotFound<ProblemDetails>>> AddIngredient(
        Guid providerId, int ingredientId,
        [FromServices] IProvidersHandler handler,
        CancellationToken ct)
    {
        var result = await handler.AddIngredient(new CreateIngredientProviderDto(providerId, ingredientId), ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }
}