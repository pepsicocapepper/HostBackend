using Api.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Intuit
{
    public static void MapIntuit(this WebApplication app)
    {
        var group = app.MapGroup("/intuit");
        group.MapGet("/auth", GetAuthorizationUrl);
        group.MapGet("/invoices", GetPaginatedInvoices);
    }

    private static async Task<Results<Ok<string>, BadRequest<ProblemDetails>>> GetAuthorizationUrl(
        [FromServices] IQbApi qbApi)
    {
        var result = await qbApi.GetAuthUrl();

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Ok<IReadOnlyCollection<object>>> GetPaginatedInvoices([FromServices] IQbApi qbApi,
        CancellationToken ct)
    {
        var result = await qbApi.GetPaginatedInvoices(new PaginationQuery(1, 10), ct);
        return TypedResults.Ok(result);
    }
}