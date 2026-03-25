using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BillItemModifierConfiguration : IEntityTypeConfiguration<BillItemModifier>
{
    public void Configure(EntityTypeBuilder<BillItemModifier> builder)
    {
        builder.ToTable("bill_item_modifier");
        builder.HasKey(bim => new { bim.BillItemId, bim.ModifierElementId }).HasName("bill_item_modifier_pkey");

        builder.Property(bim => bim.BillItemId).HasColumnName("bill_item_id").IsRequired();
        builder.Property(bim => bim.ModifierElementId).HasColumnName("modifier_id");
        builder.Property(bim => bim.Price).HasColumnName("price").IsRequired();
        builder.Property(bim => bim.Denomination).HasColumnName("denomination").IsRequired();

        builder.HasOne(bim => bim.BillItem)
            .WithMany(bi => bi.BillItemModifiers)
            .HasForeignKey(bim => bim.BillItemId)
            .HasConstraintName("bill_item_modifier_bill_item_id_fkey");

        builder.HasOne(bim => bim.ModifierElement)
            .WithMany(me => me.BillItemModifiers)
            .HasForeignKey(bim => bim.ModifierElementId)
            .HasConstraintName("bill_item_modifier_modifier_id_fkey");
    }
}