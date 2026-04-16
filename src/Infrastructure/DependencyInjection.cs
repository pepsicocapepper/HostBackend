using System.Runtime.CompilerServices;
using System.Text;
using Application.Common;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Intuit.Ipp.Core;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("HOST_DB_CONNECTION");

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString, o =>
            {
                o.MapEnum<Denomination>("denomination");
                o.MapEnum<PricingModel>("pricing_model");
            });
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContext, UserContext>();
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddSingleton<IQbApi>(_ =>
        {
            string? clientId = Environment.GetEnvironmentVariable("QB_CLIENT_ID");
            string? clientSecret = Environment.GetEnvironmentVariable("QB_CLIENT_SECRET");
            string? refreshToken = Environment.GetEnvironmentVariable("QB_REFRESH_TOKEN");
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) ||
                string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException(nameof(clientId));

            OAuth2Client client =
                new OAuth2Client(clientId, clientSecret, "http://localhost:8080/scalar/v1", "sandbox");

            return new QbApi(client, refreshToken);
        });

        builder.Services.AddAuthentication()
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
        builder.Services.AddAuthorization();
    }
}