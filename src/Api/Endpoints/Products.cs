using Application.Common.Models;
using Application.Products;
using Application.Products.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Products
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/", GetPaginatedProducts).WithName("GetProducts");
        group.MapPost("/", CreateProduct).RequireAuthorization();
    }

    private static async Task<CreatedAtRoute<int>> CreateProduct([FromServices] IProductsHandler handler,
        [FromBody] CreateProductDto dto,
        CancellationToken ct)
    {
        var id = await handler.CreateProduct(dto, ct);
        return TypedResults.CreatedAtRoute(id, "GetProducts");
    }

    private static async Task<Ok<PaginatedData<Product>>> GetPaginatedProducts([FromServices] IProductsHandler handler,
        CancellationToken ct)
    {
        var products = await handler.GetProducts(ct);
        return TypedResults.Ok(products);
    }
}