using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Item> Items { get; }
    DbSet<Menu> Menus { get; }
    DbSet<MenuItem> MenuItems { get; }
    DbSet<Bill> Bills { get; }
    DbSet<BillItem> BillItems { get; }
    DbSet<ItemIngredient> ItemIngredients { get; }
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Provider> Providers { get; }
    DbSet<IngredientProvider> IngredientProviders { get; }
    DbSet<Branch> Branches { get; }
    DbSet<BranchIngredient> BranchIngredients { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<RecipeIngredient> RecipeIngredients { get; }
    DbSet<ItemRecipe> ItemRecipes { get; }
    DbSet<BranchInventoryHistory> BranchInventoryHistories { get; }
    DbSet<UserQuickbooksCredential> UserQuickbooksCredentials { get; }
    public DbSet<ModifierElement> ModifierElements { get; }
    public DbSet<ModifierGroup> ModifierGroups { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}