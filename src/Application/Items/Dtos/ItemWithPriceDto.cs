using Application.Modifiers.Dtos;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemWithPriceDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? PosName { get; set; }
    public int? Color { get; set; }
    public List<ItemPriceDto> Price { get; set; } = [];
    public required string PricingModel { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ModifierGroupWithElementsDto> ModifierGroups { get; set; } = [];

    private class ItemWithPriceProfile : Profile
    {
        public ItemWithPriceProfile()
        {
            CreateMap<Item, ItemWithPriceDto>();
        }
    }
}