using Application.Common.Models;
using ErrorOr;

namespace Application.Common.Interfaces;

public interface IQbApi
{
    public Task<ErrorOr<string>> GetAuthUrl();

    public Task<IReadOnlyCollection<object>> GetPaginatedInvoices(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);
}