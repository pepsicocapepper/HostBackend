using Application.Common.Models;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products;

public interface IProductsHandler
{
    Task<int> CreateProduct(CreateProductDto createProductDto, CancellationToken cancellationToken = default);
    Task<PaginatedData<Product>> GetProducts(CancellationToken cancellationToken);
}