using Application.Common.Dtos;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;

namespace Application.Users;

public interface IUsersHandler
{
    Task<Guid> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<TokensDto?> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken);

    Task<TokensDto?> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}