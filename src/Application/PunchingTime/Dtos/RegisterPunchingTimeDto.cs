using AutoMapper;
using Domain.Entities;

namespace Application.Staffings.Dto;

public record RegisterPunchingTimeDto
{
    public required char InOrOut { get; set; }
    public required Guid UserId { get; set; }
}