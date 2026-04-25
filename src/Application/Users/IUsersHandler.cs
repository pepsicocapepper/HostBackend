using Application.Common.Dtos;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using ErrorOr;

namespace Application.Users;

public interface IUsersHandler
{
    Task<ErrorOr<Guid>> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<ErrorOr<TokensDto>> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken);

    Task<ErrorOr<TokensDto>> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}