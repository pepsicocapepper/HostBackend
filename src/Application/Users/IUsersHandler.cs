using Application.Common.Dtos;
using Application.Common.Models;
using Application.UserPunchTime.Dto;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Application.Users.Dtos;
using ErrorOr;

namespace Application.Users;

public interface IUsersHandler
{
    Task<ErrorOr<Guid>> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default);

    Task<TokensDto?> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken = default);

    Task<TokensDto?> RefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    Task<PaginatedData<UserDto>> GetPaginatedUsers(CancellationToken cancellationToken = default);

    Task<ErrorOr<UserDto>> GetUser(Guid id, CancellationToken cancellationToken = default);

    Task<ErrorOr<bool>> UpdateUser(Guid id, EditUserDto userDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteUser(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<int>> Punch(RegisterUserPunchTimeDto dto, CancellationToken cancellationToken = default);

    Task<PaginatedData<UserPunchTimeDto>> GetPaginatedPunches(PaginationQuery query,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<UserPunchTimeDto>> GetPunch(int id, CancellationToken cancellationToken = default);

    Task DeletePunch(int id, CancellationToken cancellationToken = default);

    Task<ErrorOr<MinimalUserPunchTimeDto>> UpdatePunch(int id,EditUserPunchTimeDto editPunchDto,CancellationToken cancellationToken=default);
}