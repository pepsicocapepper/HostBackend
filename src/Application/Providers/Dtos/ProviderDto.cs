using Application.Items.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Providers.Dtos;

public class ProviderDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    private class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            CreateMap<Provider, ProviderDto>();
        }
    }
}