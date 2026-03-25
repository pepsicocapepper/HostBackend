using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BillItemConfiguration : IEntityTypeConfiguration<BillItem>
{
    public void Configure(EntityTypeBuilder<BillItem> builder)
    {
        builder.ToTable("bill_item");
        builder.HasKey(bi => bi.Id).HasName("bill_item_pkey");

        builder.Property(bi => bi.Id).HasColumnName("id").IsRequired();
        builder.Property(bi => bi.BillId).HasColumnName("bill_id").IsRequired();
        builder.Property(bi => bi.ItemId).HasColumnName("item_id").IsRequired();
        builder.Property(bi => bi.Price).HasColumnName("price").IsRequired();
        builder.Property(bi => bi.Denomination).HasColumnName("denomination").HasColumnType("denomination")
            .IsRequired();
        builder.Property(bi => bi.Quantity).HasColumnName("quantity").IsRequired();

        builder.HasOne(bi => bi.Bill)
            .WithMany(m => m.BillItems)
            .HasForeignKey(bi => bi.BillId)
            .HasConstraintName("bill_item_bill_id_fkey")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bi => bi.Item)
            .WithMany()
            .HasForeignKey(bi => bi.ItemId)
            .HasConstraintName("bill_item_item_id_fkey")
            .OnDelete(DeleteBehavior.Restrict);
    }
}