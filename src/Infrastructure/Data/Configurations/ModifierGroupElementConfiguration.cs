using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ModifierGroupElementConfiguration : IEntityTypeConfiguration<ModifierGroupElement>
{
    public void Configure(EntityTypeBuilder<ModifierGroupElement> builder)
    {
        builder.ToTable("modifier_group_element");
        builder.HasKey(e => new { e.ModifierGroupId, e.ModifierElementId })
            .HasName("modifier_group_element_pkey");

        builder.Property(t => t.ModifierGroupId).HasColumnName("group_id").IsRequired();
        builder.Property(t => t.ModifierElementId).HasColumnName("element_id").IsRequired();

        builder.HasOne(t => t.ModifierGroup)
            .WithMany(t => t.ModifierGroupElements)
            .HasForeignKey(t => t.ModifierGroupId)
            .HasConstraintName("modifier_group_element_group_id_fkey");

        builder.HasOne(t => t.ModifierElement)
            .WithMany(t => t.ModifierGroupElements)
            .HasForeignKey(t => t.ModifierElementId)
            .HasConstraintName("modifier_group_element_element_id_fkey");
    }
}