using Application.Menus.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MenuProfile : Profile
{
    public MenuProfile()
    {
        CreateMap<Menu, MenusResponseDto>();
    }
    
}