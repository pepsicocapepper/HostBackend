using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");
        builder.HasKey(t => t.Id).HasName("product_pkey");
        builder.HasAlternateKey(t => t.Name).HasName("product_name_key");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Price).HasColumnName("price").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        builder.Property(t => t.UpdatedBy).HasColumnName("updated_by");

        builder.HasOne(t => t.CreatedByUser)
            .WithMany(t => t.CreatedProducts)
            .HasForeignKey(t => t.CreatedBy)
            .HasConstraintName("product_created_by_fkey");

        builder.HasOne(t => t.UpdatedByUser)
            .WithMany(t => t.UpdatedProducts)
            .HasForeignKey(t => t.UpdatedBy)
            .HasConstraintName("product_updated_by_fkey");
    }
}