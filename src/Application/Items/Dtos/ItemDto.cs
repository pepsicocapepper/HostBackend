using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Items.Dtos;

public class ItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public MinimalUserDto CreatedByUser { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public MinimalUserDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
        }
    }
}