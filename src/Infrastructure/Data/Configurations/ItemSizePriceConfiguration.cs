using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemSizePriceConfiguration : IEntityTypeConfiguration<ItemSizePrice>
{
    public void Configure(EntityTypeBuilder<ItemSizePrice> builder)
    {
        builder.ToTable("item_size_price");
        builder.HasKey(t => t.Id).HasName("item_size_price_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Size).HasColumnName("size").IsRequired();
        builder.Property(t => t.Price).HasColumnName("price").IsRequired();
        builder.Property(t => t.Denomination).HasColumnName("denomination").HasColumnType("denomination").IsRequired();
        builder.Property(t => t.ItemId).HasColumnName("item_id").IsRequired();

        builder.HasOne(t => t.Item)
            .WithMany(t => t.SizePrices)
            .HasForeignKey(t => t.ItemId)
            .HasConstraintName("item_size_price_item_id_fkey");
    }
}