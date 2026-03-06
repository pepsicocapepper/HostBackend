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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}