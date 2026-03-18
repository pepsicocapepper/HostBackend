using Api.Endpoints;
using Api.Services;
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddApplication();
builder.AddInfrastructure();
builder.Services.AddScoped<ILocation, CurrentLocation>();
builder.Services.AddOpenApi();

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
app.MapUsers();

app.Run();