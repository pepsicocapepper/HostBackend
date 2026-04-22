using System.Reflection;
using Application.Bills;
using Application.Branches;
using Application.Ingredients;
using Application.Intuit;
using Application.Items;
using Application.Menus;
using Application.Modifiers;
using Application.Providers;
using Application.Recipes;
using Application.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        builder.Services.AddAutoMapper(cfg => { cfg.AddMaps(Assembly.GetExecutingAssembly()); });
        builder.Services.AddScoped<IUsersHandler, UsersHandler>();
        builder.Services.AddScoped<IItemsHandler, ItemsHandler>();
        builder.Services.AddScoped<IMenusHandler, MenusHandler>();
        builder.Services.AddScoped<IBillsHandler, BillsHandler>();
        builder.Services.AddScoped<IIngredientsHandler, IngredientsHandler>();
        builder.Services.AddScoped<IProvidersHandler, ProvidersHandler>();
        builder.Services.AddScoped<IBranchesHandler, BranchesHandler>();
        builder.Services.AddScoped<IRecipesHandler, RecipesHandler>();
        builder.Services.AddScoped<IModifiersHandler, ModifiersHandler>();
        builder.Services.AddScoped<IQbHandler, QbHandler>();
    }
}