using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipe");
        builder.HasKey(p => p.Id).HasName("recipe_pkey");

        builder.Property(p => p.Id).HasColumnName("id").HasDefaultValue();
        builder.Property(p => p.Name).HasColumnName("name").IsRequired();
        builder.Property(p => p.Steps).HasColumnName("steps").IsRequired().HasDefaultValue();

        builder.HasMany(p => p.ItemRecipes)
            .WithOne(p => p.Recipe)
            .HasForeignKey(p => p.RecipeId)
            .HasConstraintName("item_recipe_recipe_id_fkey");

        builder.HasMany(p => p.RecipeIngredients)
            .WithOne(p => p.Recipe)
            .HasForeignKey(p => p.RecipeId)
            .HasConstraintName("recipe_ingredient_recipe_id_fkey");
    }
}