using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemModifierGroupConfiguration : IEntityTypeConfiguration<ItemModifierGroup>
{
    public void Configure(EntityTypeBuilder<ItemModifierGroup> builder)
    {
        builder.ToTable("item_modifier_group");
        builder.HasKey(t => t.Id).HasName("item_modifier_group_pkey");
        builder.HasIndex(e => new { e.ItemId, e.ModifierGroupId })
            .IsUnique()
            .HasDatabaseName("item_modifier_group_item_id_group_id_key");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.ModifierGroupId).HasColumnName("group_id").IsRequired();
        builder.Property(t => t.ItemId).HasColumnName("item_id").IsRequired();
        builder.Property(t => t.DisplayOrder).HasColumnName("display_order").IsRequired();

        builder.HasOne(t => t.Item)
            .WithMany(t => t.ItemModifierGroups)
            .HasForeignKey(t => t.ItemId)
            .HasConstraintName("item_modifier_group_item_id_fkey");

        builder.HasOne(t => t.ModifierGroups)
            .WithMany(t => t.ItemModifierGroups)
            .HasForeignKey(t => t.ModifierGroupId)
            .HasConstraintName("item_modifier_group_group_id_fkey");
    }
}