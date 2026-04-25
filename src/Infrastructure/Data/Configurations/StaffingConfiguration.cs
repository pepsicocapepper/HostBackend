using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class StaffingConfiguration : IEntityTypeConfiguration<Staffing>
{
    public void Configure(EntityTypeBuilder<Staffing> builder)
    {
        builder.ToTable("staffing");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd();
    }
}