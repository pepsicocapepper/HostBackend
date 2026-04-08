using Application.Common.Models;
using Application.Providers.Dtos;
using ErrorOr;

namespace Application.Providers;

public interface IProvidersHandler
{
    public Task<ErrorOr<Guid>> CreateProvider(CreateProviderDto dto, CancellationToken cancellationToken = default);
    public Task<PaginatedData<ProviderDto>> GetPaginatedProviders(CancellationToken cancellationToken = default);

    public Task<ErrorOr<bool>> AddIngredient(CreateIngredientProviderDto dto,
        CancellationToken cancellationToken = default);
}