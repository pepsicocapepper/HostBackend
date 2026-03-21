using AutoMapper;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemPriceDto
{
    public decimal Price { get; set; }
    public required string Denomination { get; set; }
    public short NegativeExponent { get; set; }

    private class ItemPriceProfile : Profile
    {
        public ItemPriceProfile()
        {
            CreateMap<ItemPrice, ItemPriceDto>()
                .ForMember(d => d.Denomination,
                    opt => opt.MapFrom(s => s.Denomination.ToString()
                    )
                );
        }
    }
}