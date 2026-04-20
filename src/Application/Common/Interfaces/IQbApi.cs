using Application.Common.Dtos;
using ErrorOr;

namespace Application.Common.Interfaces;

public interface IQbApi
{
    public Task<ErrorOr<string>> GetAuthUrl(CancellationToken ct = default);
    public Task<ErrorOr<TokensDto>> ExchangeAuthCode(string code, CancellationToken ct = default);
    public Task<ErrorOr<bool>> CreateSalesReceipt(string accessToken, CancellationToken ct = default);
}