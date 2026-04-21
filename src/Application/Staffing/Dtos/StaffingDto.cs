using AutoMapper;
using Domain.Entities;

namespace Application.Staffings.Dto;

public class StaffingDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    private class StaffingProfile : Profile
    {
        public StaffingProfile()
        {
            CreateMap<Staffing, StaffingDto>();
        }
    }
}