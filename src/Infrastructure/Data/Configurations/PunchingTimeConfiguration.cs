using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PunchingTimeConfigurataion : IEntityTypeConfiguration<PunchingTime>
{
    public void Configure(EntityTypeBuilder<PunchingTime> builder)
    {
 builder.ToTable("punching_times");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.InOrOut).HasColumnName("in_or_out").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
        builder.Property(t => t.Active).HasColumnName("active").IsRequired();
        builder.Property(t => t.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(t => t.PunchingTimes)
            .HasForeignKey(t => t.UserId)
            .HasConstraintName("punching_times_user_id_fkey");
    }
}