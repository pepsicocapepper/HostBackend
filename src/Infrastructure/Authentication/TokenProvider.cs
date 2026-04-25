using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Abstractions;
using Application.Common.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<TokenProvider> _logger;

    public TokenProvider(IConfiguration configuration, ApplicationDbContext dbContext, ILogger<TokenProvider> logger)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _logger = logger;
    }

    private async Task<string> GenerateRefreshTokenAsync(string token, User user, string? existingRefreshToken,
        CancellationToken cancellationToken = default)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        var jti = jwtToken.Id;

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            JwtId = jti,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        _logger.LogInformation(existingRefreshToken);
        if (!string.IsNullOrEmpty(existingRefreshToken))
        {
            var existingToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == existingRefreshToken, cancellationToken);
            _logger.LogInformation(existingToken?.Token);
            if (existingToken != null)
            {
                _logger.LogInformation("Token: {0}", existingToken.Token);
                _dbContext.RefreshTokens.Remove(existingToken);
            }
        }

        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshToken.Token;
    }

    private string GenerateAccessToken(User user)
    {
        string secretKey = _configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public async Task<TokensDto> GenerateTokensAsync(User user, string? existingRefreshToken,
        CancellationToken cancellationToken = default)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(accessToken, user, existingRefreshToken, cancellationToken);
        return new TokensDto(accessToken, refreshToken);
    }
}