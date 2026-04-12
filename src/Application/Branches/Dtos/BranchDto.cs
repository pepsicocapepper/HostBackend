using AutoMapper;
using Domain.Entities;

namespace Application.Branches.Dtos;

public class BranchDto
{
    public Guid Id { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string? ZipCode { get; set; }
    public string Locality { get; set; } = null!;
    public string AdministrativeArea { get; set; } = null!;
    public string Country { get; set; } = null!;
    
    private class BranchDtoProfile : Profile
    {
        public BranchDtoProfile()
        {
            CreateMap<Branch, BranchDto>();
        }
    }
}