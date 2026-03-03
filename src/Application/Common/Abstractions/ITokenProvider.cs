using Application.Common.Dtos;
using Domain.Entities;

namespace Application.Common.Abstractions;

public interface ITokenProvider
{
    Task<TokensDto> GenerateTokensAsync(User user, string? existingRefreshToken);
}