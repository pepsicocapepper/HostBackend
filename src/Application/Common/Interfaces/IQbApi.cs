using Application.Common.Dtos;
using Domain.Entities;
using ErrorOr;

namespace Application.Common.Interfaces;

public interface IQbApi
{
    public Task<ErrorOr<string>> GetAuthUrl(CancellationToken ct = default);
    public Task<ErrorOr<TokensDto>> ExchangeAuthCode(string code, CancellationToken ct = default);

    public ErrorOr<bool> CreateSalesReceipt(ICollection<Bill> bills, string accessToken);
}