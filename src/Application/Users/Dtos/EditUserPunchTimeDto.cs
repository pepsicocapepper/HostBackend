using AutoMapper;
using Domain.Entities;

namespace Application.UserPunchTime.Dto;

public record EditUserPunchTimeDto
{
    public required bool IsEntrance { get; set; }
    public DateTime CreatedAt { get; set; }

}