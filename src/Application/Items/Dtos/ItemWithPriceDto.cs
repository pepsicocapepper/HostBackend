using AutoMapper;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemWithPriceDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ItemPriceDto Price { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private class ItemWithPriceProfile : Profile
    {
        public ItemWithPriceProfile()
        {
            CreateMap<Item, ItemWithPriceDto>()
                .ForMember(d => d.Price, opt
                    => opt.MapFrom(src => src.Prices.First()
                    )
                );
        }
    }
}