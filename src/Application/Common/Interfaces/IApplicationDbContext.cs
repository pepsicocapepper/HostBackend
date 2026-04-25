using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Item> Item { get; }
    DbSet<Menu> Menus { get; }
    DbSet<MenuItem> MenuItems { get; }
    DbSet<Bill> Bills { get; }
    DbSet<BillItem> BillItems { get; }
    DbSet<ItemIngredient> ItemIngredients { get; }
    DbSet<Ingredient> Ingredients { get; }
    DbSet<Provider> Providers { get; }
    DbSet<IngredientProvider> IngredientProviders { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Domain.Entities.Staffing> Staffings { get; }
    DbSet<Domain.Entities.UserPunchTime> UserPunchTimes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}