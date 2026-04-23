using AutoMapper;
using Domain.Entities;

namespace Application.UserPunchTime.Dto;

public class UserPunchTimeDto
{
    public required int Id { get; set; }
    public required bool IsEntrance { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Guid UserId { get; set; }

    private class PunchingTimeProfile : Profile
    {
        public PunchingTimeProfile()
        {
            CreateMap<Staffing, UserPunchTimeDto>();
        }
    }
}