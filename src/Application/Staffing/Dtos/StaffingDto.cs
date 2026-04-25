using AutoMapper;

namespace Application.Staffing.Dtos;

public class StaffingDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    // public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    private class StaffingProfile : Profile
    {
        public StaffingProfile()
        {
            CreateMap<Domain.Entities.Staffing, StaffingDto>();
        }
    }
}