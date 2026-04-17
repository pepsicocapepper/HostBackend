using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dto;

public class UserDto
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Pin { get; set; }
    public string? Phone  { get; set; }
    public string JobTitle  { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid BranchId { get; set; }
    private class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}