using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("menu_item");
        builder.HasKey(mi => new {mi.MenuId, mi.ItemId}).HasName("menu_item_menu_id_item_id_key");
        
        builder.Property(mi => mi.MenuId).HasColumnName("menu_id").IsRequired();
        builder.Property(mi => mi.ItemId).HasColumnName("item_id").IsRequired();
        builder.Property(mi => mi.DisplayOrder).HasColumnName("display_order").IsRequired();
        
        builder.HasOne(mi => mi.Menu)
            .WithMany(m => m.MenuItems)
            .HasForeignKey(mi => mi.MenuId)
            .HasConstraintName("menu_item_menu_id_fkey")
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(mi => mi.Item)
            .WithMany() 
            .HasForeignKey(mi => mi.ItemId)
            .HasConstraintName("menu_item_item_id_fkey")
            .OnDelete(DeleteBehavior.Restrict);
    }
}