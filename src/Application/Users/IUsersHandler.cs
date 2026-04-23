using System.Data.SqlTypes;
using Application.Common.Dtos;
using Application.Common.Models;
using Application.UserPunchTime.Dto;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using ErrorOr;

namespace Application.Users;

public interface IUsersHandler
{
    Task<ErrorOr<Guid>>RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<TokensDto?> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken);
    

    Task<TokensDto?> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    Task<PaginatedData<UserDto>> GetPaginatedUsers(CancellationToken cancellationToken);

    Task<UserDto> GetUser(Guid id,CancellationToken cancellationToken);

    Task<EditUserDto?> EditUser(Guid id,EditUserDto userDto,CancellationToken cancellationToken);
    
    Task<bool> DeleteUser(Guid id,CancellationToken cancellationToken);
    Task<ErrorOr<int>>Punch(Guid id, RegisterUserPunchTimeDto minPunchTimeDto, CancellationToken cancellationToken);

    Task<PaginatedData<UserPunchTimeDto>> GetPaginatedPunches(CancellationToken cancellationToken);

    Task<UserPunchTimeDto?> GetPunch(int id,CancellationToken cancellationToken);

    Task<MinimalUserPunchTimeDto?> EditPunch(int id,EditUserPunchTimeDto editPunchDto,CancellationToken cancellationToken);
    
    Task<bool> DeletePunch(int id,CancellationToken cancellationToken);
    
}