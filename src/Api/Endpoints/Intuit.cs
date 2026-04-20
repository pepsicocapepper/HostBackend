using Api.Common.Extensions;
using Application.Intuit;
using Application.Intuit.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Intuit
{
    public static void MapIntuit(this WebApplication app)
    {
        var group = app.MapGroup("/intuit");
        group.MapGet("/auth", GetAuthorizationUrl);
        group.MapPost("/auth", ExchangeAuthCode).RequireAuthorization();
        group.MapPost("/sales-receipt", CreateSalesReceipt).RequireAuthorization();
    }

    private static async Task<Results<Ok<string>, BadRequest<ProblemDetails>>> GetAuthorizationUrl(
        [FromServices] IQbHandler handler)
    {
        var result = await handler.GetAuthUrl();

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<Created, BadRequest<ProblemDetails>>> ExchangeAuthCode(ExchangeAuthCodeDto dto,
        [FromServices] IQbHandler handler, CancellationToken ct)
    {
        var result = await handler.ExchangeAuthCode(dto.Code, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }

    private static async Task<Results<Created, UnauthorizedHttpResult>> CreateSalesReceipt(
        [FromServices] IQbHandler handler, CancellationToken ct)
    {
        var result = await handler.CreateSalesReceipt(Guid.NewGuid(), ct);

        if (result.IsError)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Created();
    }
}