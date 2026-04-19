using Application.Common.Dtos;
using ErrorOr;

namespace Application.Intuit;

public interface IQbHandler
{
    public Task<ErrorOr<string>> GetAuthUrl(CancellationToken ct = default);
    public Task<ErrorOr<bool>> ExchangeAuthCode(string code, CancellationToken ct = default);
}