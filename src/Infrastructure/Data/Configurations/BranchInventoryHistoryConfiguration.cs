using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BranchInventoryHistoryConfiguration : IEntityTypeConfiguration<BranchInventoryHistory>
{
    public void Configure(EntityTypeBuilder<BranchInventoryHistory> builder)
    {
        builder.ToTable("branch_inventory_history");
        builder.HasKey(h => h.Id).HasName("branch_inventory_history_pkey");

        builder.Property(h => h.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(h => h.PreviousQuantity).HasColumnName("previous_quantity").IsRequired();
        builder.Property(h => h.NewQuantity).HasColumnName("new_quantity").IsRequired();
        builder.Property(h => h.Date).HasColumnName("date").ValueGeneratedOnAdd();
        builder.Property(h => h.Action).HasColumnType("branch_inventory_action").HasColumnName("action").IsRequired();
        builder.Property(h => h.BranchId).HasColumnName("branch_id").IsRequired();
        builder.Property(h => h.IngredientId).HasColumnName("ingredient_id").IsRequired();
        builder.Property(h => h.UserId).HasColumnName("user_id").IsRequired();

        builder.HasOne(h => h.Branch)
            .WithMany(b => b.InventoryHistory)
            .HasForeignKey(h => h.BranchId)
            .HasConstraintName("branch_inventory_history_branch_id_fkey");

        builder.HasOne(h => h.Ingredient)
            .WithMany(b => b.BranchIngredientHistory)
            .HasForeignKey(h => h.IngredientId)
            .HasConstraintName("branch_inventory_history_ingredient_id_fkey");

        builder.HasOne(h => h.User)
            .WithMany(b => b.BranchInventoryHistory)
            .HasForeignKey(h => h.UserId)
            .HasConstraintName("branch_inventory_history_user_id_fkey");
    }
}