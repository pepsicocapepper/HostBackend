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
        group.MapGet("/", GetPaginatedModifiers).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<ModifierElementDto>>> GetPaginatedModifiers(int? pageNumber,
        int? pageSize,
        [FromServices] IModifiersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedModifiers(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }
}