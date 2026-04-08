using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemIngredientConfiguration : IEntityTypeConfiguration<ItemIngredient>
{
    public void Configure(EntityTypeBuilder<ItemIngredient> builder)
    {
        builder.ToTable("item_ingredient");
        builder.HasKey(t => new { t.ItemId, t.IngredientId }).HasName("item_ingredient_pkey");

        builder.Property(t => t.ItemId).HasColumnName("item_id");
        builder.Property(t => t.IngredientId).HasColumnName("ingredient_id");
        builder.Property(t => t.Quantity).HasColumnName("quantity").IsRequired();

        builder.HasOne(t => t.Ingredient)
            .WithMany()
            .HasForeignKey(t => t.IngredientId)
            .HasConstraintName("item_ingredient_ingredient_id_fkey");

        builder.HasOne(t => t.Item)
            .WithMany()
            .HasForeignKey(t => t.ItemId)
            .HasConstraintName("item_ingredient_item_id_fkey");
    }
}