using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token");
        builder.HasKey(t => t.Id).HasName("refresh_token_pkey");

        builder.Property(t => t.Id).HasColumnName("id").IsRequired();
        builder.Property(t => t.JwtId).HasColumnName("jwt_id").IsRequired();
        builder.Property(t => t.Token).HasColumnName("token").IsRequired();
        builder.Property(t => t.ExpiryDate).HasColumnName("expiry_date").IsRequired();
        builder.Property(t => t.Invalidated).HasColumnName("invalidated");
        builder.Property(t => t.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(t => t.UserId)
            .HasConstraintName("refresh_token_user_id_fkey");
    }
}