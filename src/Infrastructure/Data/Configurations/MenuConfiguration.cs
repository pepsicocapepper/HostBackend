using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("menu");
        builder.HasKey(m => m.Id).HasName("menu_pkey");
        builder.HasAlternateKey(m => m.Name).HasName("menu_name_key");

        builder.Property(m => m.Id).HasColumnName("id").IsRequired();
        builder.Property(m => m.Name).HasColumnName("name").IsRequired();
        builder.Property(m => m.PosName).HasColumnName("pos_name");
        builder.Property(m => m.Color).HasColumnName("color").HasColumnType("bytea");
        builder.Property(m => m.DisplayOrder).HasColumnName("display_order").IsRequired();
        builder.Property(m => m.ParentMenuId).HasColumnName("menu_id");

        builder.HasOne(m => m.ParentMenu)
            .WithMany(m => m.SubMenus)
            .HasForeignKey(f => f.ParentMenuId).HasConstraintName("menu_menu_id_fkey")
            .OnDelete(DeleteBehavior.Restrict);
    }
}