using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Entities;
using ErrorOr;

namespace Application.Intuit;

internal class QbHandler : IQbHandler
{
    private readonly IQbApi _qbApi;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;

    public QbHandler(IQbApi qbApi, IApplicationDbContext dbContext, IUserContext userContext)
    {
        _qbApi = qbApi;
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<ErrorOr<string>> GetAuthUrl(CancellationToken ct = default)
    {
        return await _qbApi.GetAuthUrl(ct);
    }

    public async Task<ErrorOr<bool>> ExchangeAuthCode(string code, CancellationToken ct = default)
    {
        var result = await _qbApi.ExchangeAuthCode(code, ct);

        if (result.IsError)
        {
            return ErrorOr<bool>.From(result.Errors);
        }

        var credentials = new UserQuickbooksCredential
        {
            UserId = _userContext.UserId!.Value,
            AccessToken = result.Value.AccessToken,
            RefreshToken = result.Value.RefreshToken
        };

        await _dbContext
            .UserQuickbooksCredentials
            .AddAsync(credentials, ct);
        await _dbContext.SaveChangesAsync(ct);

        return true;
    }

    public async Task<ErrorOr<bool>> CreateSalesReceipt(Guid billId, CancellationToken ct = default)
    {
        var credentials = await _dbContext
            .UserQuickbooksCredentials
            .FindAsync([_userContext.UserId!.Value], ct);

        if (credentials == null)
        {
            return Error.NotFound(IntuitErrorCodes.NotFound);
        }

        return await _qbApi.CreateSalesReceipt(credentials.AccessToken, ct);
    }
}