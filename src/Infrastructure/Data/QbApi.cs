using Application.Common.Dtos;
using Application.Common.Interfaces;
using ErrorOr;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using Bill = Domain.Entities.Bill;
using Error = ErrorOr.Error;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Data;

public sealed class QbApi(OAuth2Client client, string realmId) : IQbApi
{
    DataService CreateDataService(string accessToken)
    {
        var requestValidator = new OAuth2RequestValidator(accessToken);
        var context = new ServiceContext(realmId, IntuitServicesType.QBO, requestValidator);
        context.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/";
        context.IppConfiguration.MinorVersion.Qbo = "75";
        return new DataService(context);
    }

    public Task<ErrorOr<string>> GetAuthUrl(CancellationToken ct = default)
    {
        string? clientId = Environment.GetEnvironmentVariable("QB_CLIENT_ID");
        string? clientSecret = Environment.GetEnvironmentVariable("QB_CLIENT_SECRET");
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            return Task.FromResult<ErrorOr<string>>(Error.Custom(0, "Unconfigured", "Not configured"));
        }

        List<OidcScopes> scopes =
        [
            OidcScopes.Accounting,
            OidcScopes.Phone,
            OidcScopes.Email,
            OidcScopes.OpenId
        ];

        return Task.FromResult<ErrorOr<string>>(client.GetAuthorizationURL(scopes));
    }

    public async Task<ErrorOr<TokensDto>> ExchangeAuthCode(string code, CancellationToken ct = default)
    {
        var response = await client.GetBearerTokenAsync(code, ct);

        if (response.IsError)
        {
            return Error.Unexpected(response.Error);
        }

        return new TokensDto(response.AccessToken, response.RefreshToken);
    }


    public ErrorOr<bool> CreateSalesReceipt(ICollection<Bill> bills, string accessToken)
    {
        var dataService = CreateDataService(accessToken);
        dataService.Add(new SalesReceipt
        {
            CustomerRef = new ReferenceType { Value = "1" },
            TxnDate = DateTime.UtcNow,
            TxnDateSpecified = true,
            Line = bills.Select((bill, index) => new Line
            {
                Description = $"Bill #{index} - {bill.CreatedByUser.Name}",
                DetailType = LineDetailTypeEnum.SalesItemLineDetail,
                AnyIntuitObject = new SalesItemLineDetail
                {
                    TaxCodeRef = new ReferenceType
                    {
                        Value = "NON"
                    },
                    Qty = 1,
                    QtySpecified = true,
                    ServiceDate = bill.CreatedAt,
                },
                Amount = bill.Amount,
                AmountSpecified = true,
                DetailTypeSpecified = true,
            }).ToArray()
        });
        return true;
    }

    // public async Task<IReadOnlyCollection<object>> GetPaginatedInvoices(PaginationQuery paginationQuery,
    //     CancellationToken cancellationToken = default)
    // {
    //     var polly = Policy
    //         .Handle<InvalidTokenException>()
    //         .RetryAsync(1, async (_, _) =>
    //         {
    //             _accessToken = null;
    //             await GetAccessToken();
    //         });
    //
    //     return await polly.ExecuteAsync(async () =>
    //     {
    //         string? realmId = Environment.GetEnvironmentVariable("QB_REALM_ID");
    //
    //         if (realmId == null)
    //         {
    //             throw new NullReferenceException("QB_REALM_ID not configured");
    //         }
    //
    //         OAuth2RequestValidator validator = new OAuth2RequestValidator(await GetAccessToken());
    //         ServiceContext ctx = new ServiceContext(realmId, IntuitServicesType.QBO, validator);
    //         ctx.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/";
    //         ctx.IppConfiguration.MinorVersion.Qbo = "75";
    //         DataService service = new DataService(ctx);
    //
    //         return await Task.FromResult<IReadOnlyCollection<object>>(service.FindAll(new Invoice()));
    //     });
    // }
}