using Application.Bills;
using Application.Bills.Dtos;
using Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Bills
{
    public static void MapBills(this WebApplication app)
    {
        var group = app.MapGroup("/bills");

        group.MapGet("/", GetPaginatedBills).RequireAuthorization();
        group.MapPost("/", CreateBill).RequireAuthorization();
    }

    private static async Task<Ok<PaginatedData<BillDto>>> GetPaginatedBills(
        [FromServices] IBillsHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.GetPaginatedBillsAsync(cancellationToken);
        return TypedResults.Ok(result);
    }

    private static async Task<Created> CreateBill(
        [FromServices] IBillsHandler handler,
        [FromBody] CreateBillDto dto,
        CancellationToken cancellationToken)
    {
        await handler.CreateBill(dto, cancellationToken);
        return TypedResults.Created();
    }
}