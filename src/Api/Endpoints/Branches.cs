using Api.Common.Extensions;
using Application.Branches;
using Application.Branches.Dtos;
using Application.Common.Models;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Branches
{
    public static void MapBranches(this WebApplication app)
    {
        var group = app.MapGroup("/branches");
        group.MapGet("/", GetPaginatedBranches).RequireAuthorization();
        group.MapGet("/{id:guid}", GetBranchById).RequireAuthorization();
        group.MapGet("/{branchId:guid}/users", GetPaginatedBranchUsers).RequireAuthorization();
        group.MapPost("/", CreateBranch).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<BranchDto>>> GetPaginatedBranches(int? pageNumber, int? pageSize,
        [FromServices] IBranchesHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPaginatedBranches(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<BranchDto>, NotFound<ProblemDetails>>> GetBranchById(
        Guid id,
        [FromServices] IBranchesHandler handler,
        CancellationToken ct)
    {
        var result = await handler.GetBranchById(id, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<Ok<PaginatedData<MinimalUserDto>>, NotFound<ProblemDetails>>>
        GetPaginatedBranchUsers(int? pageNumber, int? pageSize, Guid branchId,
            [FromServices] IBranchesHandler handler, CancellationToken ct
        )
    {
        var result = await handler.GetPaginatedBranchUsers(new PaginationQuery(pageNumber, pageSize), branchId, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
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