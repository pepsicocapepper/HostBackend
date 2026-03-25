using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ModifierGroupConfiguration : IEntityTypeConfiguration<ModifierGroup>
{
    public void Configure(EntityTypeBuilder<ModifierGroup> builder)
    {
        builder.ToTable("modifier_group");
        builder.HasKey(t => t.Id).HasName("modifier_group_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
    }
}