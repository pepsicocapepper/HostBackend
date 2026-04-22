using AutoMapper;
using Domain.Entities;

namespace Application.Staffings.Dto;

public class PunchingTimeDto
{
    public required Guid Id { get; set; }
    public required string InOrOut { get; set; }
    public DateTime CreatedAt { get; set; }
    public required bool Active {get;set;}
    private class PunchingTimeProfile : Profile
    {
        public PunchingTimeProfile()
        {
            CreateMap<Staffing, PunchingTimeDto>();
        }
    }
}