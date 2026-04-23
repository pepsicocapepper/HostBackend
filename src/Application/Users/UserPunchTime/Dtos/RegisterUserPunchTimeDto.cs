using AutoMapper;
using Domain.Entities;

namespace Application.UserPunchTime.Dto;

public record RegisterUserPunchTimeDto
{
    public required bool IsEntrance { get; set; }
}