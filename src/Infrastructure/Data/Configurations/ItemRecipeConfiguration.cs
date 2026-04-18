using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemRecipeConfiguration : IEntityTypeConfiguration<ItemRecipe>
{
    public void Configure(EntityTypeBuilder<ItemRecipe> builder)
    {
        builder.ToTable("item_recipe");
        builder.HasKey(p => new { p.RecipeId, p.ItemId }).HasName("item_recipe_pkey");

        builder.Property(p => p.ItemId).HasColumnName("item_id");
        builder.Property(p => p.RecipeId).HasColumnName("recipe_id");
    }
}