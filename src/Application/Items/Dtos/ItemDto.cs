using AutoMapper;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public Guid CreatedBy { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public User? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }
}