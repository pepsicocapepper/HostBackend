using System.Data.SqlTypes;
using Application.Common.Dtos;
using Application.Common.Models;
using Application.Staffings.Dto;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using ErrorOr;

namespace Application.Users;

public interface IStaffingHandler
{
    // Task<ErrorOr<Guid>>Register(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<PaginatedData<StaffingDto>> GetPaginatedStaffings(CancellationToken cancellationToken);

    Task<StaffingDto?> GetStaffing(Guid id,CancellationToken cancellationToken);

    // Task<EditUserDto?> EditUser(Guid id,EditUserDto userDto,CancellationToken cancellationToken);
    
    // Task<bool> DeleteUser(Guid id,CancellationToken cancellationToken);
    
}