using Application.Common;
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
    public DbSet<Item> Item => Set<Item>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Staffing> Staffings => Set<Staffing>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();
    public DbSet<ItemIngredient> ItemIngredients => Set<ItemIngredient>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<IngredientProvider> IngredientProviders => Set<IngredientProvider>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<UserPunchTime> UserPunchTimes => Set<UserPunchTime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<Denomination>("denomination");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        // =======================================
        modelBuilder.Entity<User>().Property(b => b.CreatedAt).ValueGeneratedOnAdd();
        modelBuilder.Entity<UserPunchTime>().Property(b => b.CreatedAt).ValueGeneratedOnAdd();
        // ^^^^^^
        // le dice al entity framework que usuario no recibira nada en CreatedAt
    }
}