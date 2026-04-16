using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BranchIngredientConfiguration : IEntityTypeConfiguration<BranchIngredient>
{
    public void Configure(EntityTypeBuilder<BranchIngredient> builder)
    {
        builder.ToTable("branch_ingredient");
        builder.HasKey(t => new { t.IngredientId, t.BranchId }).HasName("branch_ingredient_pkey");

        builder.Property(t => t.IngredientId).HasColumnName("ingredient_id").IsRequired();
        builder.Property(t => t.BranchId).HasColumnName("branch_id").IsRequired();
        builder.Property(t => t.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(t => t.Unit).HasColumnName("unit").IsRequired();

        builder.HasOne(t => t.Ingredient)
            .WithMany(t => t.BranchIngredients)
            .HasForeignKey(t => t.IngredientId)
            .HasConstraintName("branch_ingredient_ingredient_id_fkey");

        builder.HasOne(t => t.Branch)
            .WithMany(t => t.BranchIngredients)
            .HasForeignKey(t => t.BranchId)
            .HasConstraintName("branch_ingredient_branch_id_fkey");
    }
}