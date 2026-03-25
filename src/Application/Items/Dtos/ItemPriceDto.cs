using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemPriceDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    public decimal Price { get; set; }
    public required string Denomination { get; set; }

    private class ItemPriceProfile : Profile
    {
        public ItemPriceProfile()
        {
            CreateMap<ItemBasePrice, ItemPriceDto>()
                .ForMember(d => d.Denomination,
                    opt => opt.MapFrom(s => s.Denomination.ToString()
                    )
                );
        }
    }
}