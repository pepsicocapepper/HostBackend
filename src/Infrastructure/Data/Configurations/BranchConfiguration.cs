using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("branch");
        builder.HasKey(p => p.Id).HasName("branch_pkey");

        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.AddressLine1).HasColumnName("address_line_1").IsRequired();
        builder.Property(p => p.AddressLine2).HasColumnName("address_line_2");
        builder.Property(p => p.ZipCode).HasColumnName("zip_code");
        builder.Property(p => p.Locality).HasColumnName("locality").IsRequired();
        builder.Property(p => p.AdministrativeArea).HasColumnName("administrative_area").IsRequired();
        builder.Property(p => p.Country).HasColumnName("country").IsRequired();
    }
}