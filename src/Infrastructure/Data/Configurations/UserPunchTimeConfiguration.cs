using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PunchingTimeConfiguration : IEntityTypeConfiguration<UserPunchTime>
{
    public void Configure(EntityTypeBuilder<UserPunchTime> builder)
    {
        builder.ToTable("user_punch_time");
        builder.HasKey(t => t.Id).HasName("user_punch_time_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.IsEntrance).HasColumnName("is_entrance").IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd();
        builder.Property(t => t.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(t => t.UserPunchTimes)
            .HasForeignKey(t => t.UserId)
            .HasConstraintName("user_punch_time_user_id_fkey");
    }
}