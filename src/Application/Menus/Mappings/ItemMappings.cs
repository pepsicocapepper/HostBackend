using System.Buffers.Binary;
using System.Linq.Expressions;
using Application.Items.Dtos;
using Application.Modifiers.Dtos;
using Domain.Common;
using Domain.Entities;

namespace Application.Menus.Mappings;

public static class ItemMappings
{
    public static Expression<Func<MenuItem, ItemWithPriceDto>> ToItemWithPriceDto(
        Denomination? denomination
    ) =>
        mi => new ItemWithPriceDto
        {
            Id = mi.Item.Id,
            Name = mi.Item.Name,
            PosName = mi.Item.PosName,
            Color = mi.Item.Color != null && mi.Item.Color.Length > 0
                ? BinaryPrimitives.ReadInt32BigEndian(mi.Item.Color)
                : null,
            Price = mi.Item.PricingModel == PricingModel.Base
                ? mi.Item.BasePrices
                    .Select(p => new ItemPriceDto
                    {
                        Price = p.Price,
                        Denomination = p.Denomination.ToString()
                    })
                    .ToList()
                : mi.Item.SizePrices
                    .OrderBy(p => p.Price)
                    .Select(p => new ItemPriceDto
                    {
                        Size = p.Size,
                        Price = p.Price,
                        Denomination = p.Denomination.ToString()
                    })
                    .ToList(),
            PricingModel = mi.Item.PricingModel.ToString(),
            CreatedAt = mi.Item.CreatedAt,
            CreatedBy = mi.Item.CreatedBy,
            UpdatedAt = mi.Item.UpdatedAt,
            UpdatedBy = mi.Item.UpdatedBy,
            ModifierGroups = mi.Item.ItemModifierGroups.Select(img => img.ModifierGroups)
                .Select(mg => new ModifierGroupDto
                {
                    Id = mg.Id,
                    Name = mg.Name,
                    ModifierElements = mg.ModifierGroupElements.Select(mge => mge.ModifierElement)
                        .Select(me => new ModifierElementDto
                        {
                            Id = me.Id,
                            Name = me.Name,
                            Price = me.Price,
                            Denomination = me.Denomination.ToString()
                        })
                        .ToList()
                })
                .ToList()
        };
}