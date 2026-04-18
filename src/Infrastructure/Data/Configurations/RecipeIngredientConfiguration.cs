using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable("recipe_ingredient");
        builder.HasKey(p => new { p.RecipeId, p.IngredientId }).HasName("recipe_ingredient_pkey");

        builder.Property(p => p.RecipeId).HasColumnName("recipe_id");
        builder.Property(p => p.IngredientId).HasColumnName("ingredient_id");
        builder.Property(p => p.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(p => p.Unit).HasColumnName("unit").IsRequired();
    }
}