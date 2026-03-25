using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ModifierElementConfiguration : IEntityTypeConfiguration<ModifierElement>
{
    public void Configure(EntityTypeBuilder<ModifierElement> builder)
    {
        builder.ToTable("modifier_element");
        builder.HasKey(t => t.Id).HasName("modifier_element_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Price).HasColumnName("price").IsRequired();
        builder.Property(t => t.Denomination).HasColumnName("denomination")
            .HasColumnType("denomination")
            .IsRequired();
    }
}