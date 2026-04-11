using Api.Common.Extensions;
using Application.Branches;
using Application.Branches.Dtos;
using Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Branches
{
    public static void MapBranches(this WebApplication app)
    {
        var group = app.MapGroup("/branches");
        group.MapGet("/", GetPaginatedBranches).RequireAuthorization();
        group.MapPost("/", CreateBranch).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<BranchDto>>> GetPaginatedBranches(int? pageNumber, int? pageSize,
        [FromServices] IBranchesHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedBranches(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Created, BadRequest<ProblemDetails>>> CreateBranch(
        [FromBody] CreateBranchDto branchDto,
        [FromServices] IBranchesHandler handler,
        CancellationToken ct)
    {
        var result = await handler.CreateBranch(branchDto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }
}