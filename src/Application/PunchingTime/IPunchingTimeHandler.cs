using System.Data.SqlTypes;
using Application.Common.Dtos;
using Application.Common.Models;
using Application.Staffings.Dto;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Domain.Entities;
using ErrorOr;

namespace Application.Users;

public interface IPunchingTimeHandler
{
    Task<ErrorOr<Guid>>Punch(RegisterPunchingTimeDto punchingTimeDto, CancellationToken cancellationToken);

    Task<PaginatedData<PunchingTimeDto>> GetPaginatedPunches(CancellationToken cancellationToken);

    Task<EditPunchingTimeDto?> EditPunch(Guid id,EditPunchingTimeDto userDto,CancellationToken cancellationToken);
    
    Task<bool> DeletePunch(Guid id,CancellationToken cancellationToken);
    
}