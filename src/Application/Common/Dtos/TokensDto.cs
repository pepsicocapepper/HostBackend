namespace Application.Common.Dtos;

public record TokensDto(
    string AccessToken,
    string RefreshToken
);