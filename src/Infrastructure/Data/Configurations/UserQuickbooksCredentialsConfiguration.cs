using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserQuickbooksCredentialsConfiguration : IEntityTypeConfiguration<UserQuickbooksCredential>
{
    public void Configure(EntityTypeBuilder<UserQuickbooksCredential> builder)
    {
        builder.ToTable("host_user_quickbooks_credentials");
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(u => u.AccessToken).HasColumnName("access_token").IsRequired();
        builder.Property(u => u.RefreshToken).HasColumnName("refresh_token").IsRequired();

        builder.HasOne(u => u.User)
            .WithOne(u => u.QuickbooksCredentials)
            .HasForeignKey<UserQuickbooksCredential>(u => u.UserId)
            .HasConstraintName("host_user_quickbooks_credentials_user_id_fkey");
    }
}