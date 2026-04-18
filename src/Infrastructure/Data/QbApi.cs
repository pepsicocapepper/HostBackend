using Application.Common.Interfaces;
using Application.Common.Models;
using ErrorOr;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Exception;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using Polly;
using Error = ErrorOr.Error;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Data;

public sealed class QbApi(OAuth2Client client, string refreshToken) : IQbApi
{
    private string? _accessToken;
    private string _refreshToken = refreshToken;

    private async Task<string> GetAccessToken()
    {
        if (_accessToken == null)
        {
            var resp = await client.RefreshTokenAsync(_refreshToken);
            _accessToken = resp.AccessToken;
            _refreshToken = resp.RefreshToken;
        }

        return _accessToken;
    }

    public Task<ErrorOr<string>> GetAuthUrl()
    {
        string? clientId = Environment.GetEnvironmentVariable("QB_CLIENT_ID");
        string? clientSecret = Environment.GetEnvironmentVariable("QB_CLIENT_SECRET");
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            return Task.FromResult<ErrorOr<string>>(Error.Custom(0, "Unconfigured", "Not configured"));
        }

        OAuth2Client client = new OAuth2Client(clientId, clientSecret, "http://localhost:8080/scalar/v1", "sandbox");
        List<OidcScopes> scopes =
        [
            OidcScopes.Accounting,
            OidcScopes.Phone,
            OidcScopes.Email,
            OidcScopes.OpenId
        ];

        return Task.FromResult<ErrorOr<string>>(client.GetAuthorizationURL(scopes));
    }

    public async Task<IReadOnlyCollection<object>> GetPaginatedInvoices(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        var polly = Policy
            .Handle<InvalidTokenException>()
            .RetryAsync(1, async (_, _) =>
            {
                _accessToken = null;
                await GetAccessToken();
            });

        return await polly.ExecuteAsync(async () =>
        {
            string? realmId = Environment.GetEnvironmentVariable("QB_REALM_ID");

            if (realmId == null)
            {
                throw new NullReferenceException("QB_REALM_ID not configured");
            }

            OAuth2RequestValidator validator = new OAuth2RequestValidator(await GetAccessToken());
            ServiceContext ctx = new ServiceContext(realmId, IntuitServicesType.QBO, validator);
            ctx.IppConfiguration.BaseUrl.Qbo = "https://sandbox-quickbooks.api.intuit.com/";
            ctx.IppConfiguration.MinorVersion.Qbo = "75";
            DataService service = new DataService(ctx);

            return await Task.FromResult<IReadOnlyCollection<object>>(service.FindAll(new Invoice()));
        });
    }
}