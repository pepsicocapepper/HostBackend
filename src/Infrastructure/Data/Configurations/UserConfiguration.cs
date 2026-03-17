using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("host_user");
        builder.HasKey(t => t.Id).HasName("host_user_pkey");
        builder.HasAlternateKey(t => t.Pin).HasName("host_user_pin_key");
        builder.HasIndex(t => t.Pin).IsUnique().HasDatabaseName("host_user_pin_key");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Surname).HasColumnName("surname").IsRequired();
                builder.Property(t => t.Job_Title).HasColumnName("job_title").IsRequired();
        builder.Property(t => t.Pin).HasColumnName("pin").IsRequired();
        builder.Property(t => t.Active).HasColumnName("active").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
    }
}