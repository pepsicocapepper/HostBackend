using AutoMapper;
using Domain.Entities;

namespace Application.Staffings.Dto;

public record EditPunchingTimeDto
{
    public required char InOrOut { get; set; }
    public required Guid UserId { get; set; }
    public bool Active { get; set; }
}