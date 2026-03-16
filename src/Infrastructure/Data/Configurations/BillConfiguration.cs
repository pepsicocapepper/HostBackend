using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("bill");
        builder.HasKey(c => c.Id).HasName("id");

        builder.Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(c => c.Amount).HasColumnName("amount").IsRequired();
        builder.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
        
        builder.HasOne(t => t.CreatedByUser)
            .WithMany(t => t.Bills)
            .HasForeignKey(t => t.CreatedBy)
            .HasConstraintName("bill_created_by_fkey");
    }
}