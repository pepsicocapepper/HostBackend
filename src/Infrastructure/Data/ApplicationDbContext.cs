using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();
    public DbSet<ItemIngredient> ItemIngredients => Set<ItemIngredient>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<IngredientProvider> IngredientProviders => Set<IngredientProvider>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<BranchIngredient> BranchIngredients => Set<BranchIngredient>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<ItemRecipe> ItemRecipes => Set<ItemRecipe>();
    public DbSet<BranchInventoryHistory> BranchInventoryHistories => Set<BranchInventoryHistory>();
    public DbSet<UserQuickbooksCredential> UserQuickbooksCredentials => Set<UserQuickbooksCredential>();
    public DbSet<ModifierElement> ModifierElements => Set<ModifierElement>();
    public DbSet<ModifierGroup> ModifierGroups => Set<ModifierGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<Denomination>("denomination");
        modelBuilder.HasPostgresEnum<BranchInventoryAction>("branch_inventory_action");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}