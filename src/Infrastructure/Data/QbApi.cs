using Application.Common.Dtos;
using Application.Common.Interfaces;
using ErrorOr;
using Intuit.Ipp.OAuth2PlatformClient;
using Error = ErrorOr.Error;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Data;

public sealed class QbApi(OAuth2Client client) : IQbApi
{
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