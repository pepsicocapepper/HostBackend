using Api.Common.Extensions;
using Application.Common.Models;
using Application.Staffing;
using Application.Staffing.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Staffings
{
    public static void MapStaffing(this WebApplication app)
    {
        var group = app.MapGroup("/staffing");

        group.MapGet("/", GetPaginatedStaffings).RequireAuthorization();
        group.MapGet("/{id:guid}", GetStaffing).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<StaffingDto>>> GetPaginatedStaffings(
        int? pageNumber, int? pageSize, [FromServices] IStaffingHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedStaffings(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<StaffingDto>, NotFound<ProblemDetails>>>
        GetStaffing([FromServices] IStaffingHandler handler, CancellationToken ct, Guid id)
    {
        var result = await handler.GetStaffing(id, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }
}