using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Bills.Dtos;

public class BillDto
{
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    private class BillMapper : Profile
    {
        public BillMapper()
        {
            CreateMap<Bill, BillDto>();
        }
    }
}