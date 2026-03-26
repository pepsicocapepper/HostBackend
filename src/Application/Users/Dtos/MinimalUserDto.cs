using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dtos;

public sealed class MinimalUserDto
{
    public required string Name { get; set; }
    public required string Surname { get; set; }

    private class MinimalUserProfile : Profile
    {
        public MinimalUserProfile()
        {
            CreateMap<User, MinimalUserDto>();
        }
    }
}