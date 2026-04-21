using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
 builder.ToTable("host_user");
        // builder.HasKey(t => t.Id).HasName("host_user_pkey");
        // builder.HasAlternateKey(t => t.Pin).HasName("host_user_pin_key");
        builder.HasIndex(t => t.Pin).IsUnique().HasDatabaseName("host_user_pin_key");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Surname).HasColumnName("surname").IsRequired();
        builder.Property(t => t.JobTitle).HasColumnName("job_title").IsRequired();
        builder.Property(t => t.Phone).HasColumnName("phone");
        builder.Property(t => t.Pin).HasColumnName("pin").IsRequired();
        builder.Property(t => t.Active).HasColumnName("active");
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
         builder.Property(t => t.StaffingId).HasColumnName("staffing_id").IsRequired();

        builder.HasOne(t => t.Staffing)
            .WithMany(t => t.Users)
            .HasForeignKey(t => t.StaffingId)
            .HasConstraintName("host_user_staffing_id_fkey");

        builder.Property(t => t.BranchId).HasColumnName("branch_id").IsRequired();

        builder.HasOne(t => t.Branch)
            .WithMany(t => t.Users)
            .HasForeignKey(t => t.BranchId)
            .HasConstraintName("host_user_branch_id_fkey");
    }
}