using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("item");
        builder.HasKey(t => t.Id).HasName("item_pkey");
        builder.HasAlternateKey(t => t.Name).HasName("item_name_key");

        builder.Property(t => t.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.PosName).HasColumnName("pos_name");
        builder.Property(t => t.Plu).HasColumnName("plu");
        builder.Property(t => t.Sku).HasColumnName("sku");
        builder.Property(t => t.Description).HasColumnName("description");
        builder.Property(t => t.Color).HasColumnName("color").HasColumnType("bytea");
        builder.Property(t => t.PricingModel).HasColumnName("pricing_model").HasColumnType("pricing_model")
            .IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        builder.Property(t => t.UpdatedBy).HasColumnName("updated_by");
        builder.Property(t => t.IsActive).HasColumnName("is_active").IsRequired();

        builder.HasOne(t => t.CreatedByUser)
            .WithMany(t => t.CreatedItems)
            .HasForeignKey(t => t.CreatedBy)
            .HasConstraintName("item_created_by_fkey")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.UpdatedByUser)
            .WithMany(t => t.UpdatedItems)
            .HasForeignKey(t => t.UpdatedBy)
            .HasConstraintName("item_updated_by_fkey")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.ItemIngredients)
            .WithOne(t => t.Item)
            .HasForeignKey(t => t.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        
    }
}