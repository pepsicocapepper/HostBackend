using System.Text.Json.Serialization;
using Api.Common;
using Api.Endpoints;
using Api.Services;
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddApplication();
builder.AddInfrastructure();
builder.Services.AddProblemDetails();
builder.Services.AddValidation();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<ILocation, CurrentLocation>();
builder.Services.AddOpenApi();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapMenus();
app.MapAuth();
app.MapProducts();
app.MapBills();
app.MapProviders();
app.MapIngredients();
app.MapBranches();

app.Run();