using System.Data.SqlTypes;
using Application.Common.Dtos;
using Application.Common.Models;
using Application.UserPunchTime.Dto;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Domain.Entities;
using ErrorOr;

namespace Application.PunchingTime;

public interface IUserPunchTimeHandler
{
    Task<ErrorOr<int>>Punch(Guid id, RegisterUserPunchTimeDto minPunchTimeDto, CancellationToken cancellationToken);

    Task<UserPunchTimeDto?> GetPaginatedPunches(int id, CancellationToken cancellationToken);

    Task<UserPunchTimeDto?> GetPunch(int id,CancellationToken cancellationToken);

    Task<EditUserPunchTimeDto?> EditPunch(int id,EditUserPunchTimeDto editPunchDto,CancellationToken cancellationToken);
    
    Task<bool> DeletePunch(int id,CancellationToken cancellationToken);
    
}