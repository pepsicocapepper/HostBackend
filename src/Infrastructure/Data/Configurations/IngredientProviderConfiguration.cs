using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class IngredientProviderConfiguration : IEntityTypeConfiguration<IngredientProvider>
{
    public void Configure(EntityTypeBuilder<IngredientProvider> builder)
    {
        builder.ToTable("ingredient_provider");
        builder.HasKey(t => new { t.IngredientId, t.ProviderId }).HasName("ingredient_provider_pkey");

        builder.Property(t => t.IngredientId).HasColumnName("ingredient_id").IsRequired();
        builder.Property(t => t.ProviderId).HasColumnName("provider_id").IsRequired();
        builder.Property(t => t.Price).HasColumnName("price").IsRequired();

        builder.HasOne(t => t.Ingredient)
            .WithMany(t => t.ProviderIngredients)
            .HasForeignKey(t => t.IngredientId)
            .HasConstraintName("ingredient_provider_ingredient_id_fkey");

        builder.HasOne(t => t.Provider)
            .WithMany(t => t.ProviderIngredients)
            .HasForeignKey(t => t.ProviderId)
            .HasConstraintName("ingredient_provider_provider_id_fkey");
    }
}