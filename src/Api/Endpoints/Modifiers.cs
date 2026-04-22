using Api.Common.Extensions;
using Application.Common.Models;
using Application.Modifiers;
using Application.Modifiers.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Modifiers
{
    public static void MapModifiers(this WebApplication app)
    {
        var group = app.MapGroup("/modifiers");
        group.MapGet("/elements", GetPaginatedModifierElements).RequireAuthorization();
        group.MapGet("/elements/{modId:guid}/available-mod-groups", GetPaginatedModGroupsNotInModElement)
            .RequireAuthorization();
        group.MapGet("/groups", GetPaginatedModifierGroups).RequireAuthorization();
        group.MapPost("/elements", CreateModifierElement).RequireAuthorization();
        group.MapPost("/groups", CreateModifierGroup).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<ModifierElementDto>>> GetPaginatedModifierElements(int? pageNumber,
        int? pageSize,
        [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedModifierElements(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<PaginatedData<ModifierGroupDto>>, NotFound<ProblemDetails>>>
        GetPaginatedModGroupsNotInModElement(int? pageNumber, int? pageSize, Guid modId,
            [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        var result =
            await handler.GetPaginatedModGroupsNotInModElement(new PaginationQuery(pageNumber, pageSize), modId, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<PaginatedData<ModifierGroupWithElementsDto>>> GetPaginatedModifierGroups(
        int? pageNumber,
        int? pageSize,
        [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedModifierGroups(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Created> CreateModifierGroup([FromBody] CreateModifierGroupDto dto,
        [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        await handler.CreateModifierGroup(dto, ct);
        return TypedResults.Created();
    }


    private static async Task<Created> CreateModifierElement([FromBody] CreateModifierElementDto dto,
        [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        await handler.CreateModifierElement(dto, ct);
        return TypedResults.Created();
    }
}