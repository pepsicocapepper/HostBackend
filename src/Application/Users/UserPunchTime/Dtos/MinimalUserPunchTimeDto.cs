using AutoMapper;
using Domain.Entities;

namespace Application.UserPunchTime.Dto;

public record MinimalUserPunchTimeDto
{
    public required bool IsEntrance { get; set; }
}