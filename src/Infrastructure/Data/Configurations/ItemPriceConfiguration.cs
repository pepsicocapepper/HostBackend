using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemPriceConfiguration : IEntityTypeConfiguration<ItemPrice>
{
    public void Configure(EntityTypeBuilder<ItemPrice> builder)
    {
        builder.ToTable("item_price");
        builder.HasKey(t => t.Id).HasName("item_price_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Price).HasColumnName("price").IsRequired();
        builder.Property(t => t.Denomination).HasColumnName("denomination").HasColumnType("denomination").IsRequired();
        builder.Property(t => t.ItemId).HasColumnName("item_id").IsRequired();

        builder.HasOne(t => t.Item)
            .WithMany(t => t.Prices)
            .HasForeignKey(t => t.ItemId)
            .HasConstraintName("item_price_item_id_fkey");
    }
}