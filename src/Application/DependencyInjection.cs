using System.Reflection;
using Application.Products;
using Application.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });
        builder.Services.AddScoped<IUsersHandler, UsersHandler>();
        builder.Services.AddScoped<IProductsHandler, ProductsHandler>();
    }
}