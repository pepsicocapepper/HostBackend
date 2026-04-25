using Application.Common.Dtos;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dtos;
using ErrorOr;

namespace Application.Users;

public interface IUsersHandler
{
    Task<ErrorOr<MinimalUserDto>> GetSelfInfo(CancellationToken cancellationToken = default);
    Task<ErrorOr<Guid>> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default);

    Task<ErrorOr<TokensDto>> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken = default);

    Task LogoutUser(string refreshToken, CancellationToken cancellationToken = default);

    Task<ErrorOr<TokensDto>> RefreshToken(string refreshToken, CancellationToken cancellationToken = default);
}