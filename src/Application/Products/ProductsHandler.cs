using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products;

internal class ProductsHandler : IProductsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;

    public ProductsHandler(IApplicationDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<int> CreateProduct(CreateProductDto createProductDto,
        CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            CreatedBy = _userContext.UserId!.Value
        };

        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

    public Task<PaginatedData<Product>> GetProducts(CancellationToken cancellationToken = default)
    {
        var products = _dbContext
            .Products
            .PaginatedListAsync(1, 10, cancellationToken);
        return products;
    }
}