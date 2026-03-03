using Application.Common.Dtos;
using Application.Users;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Auth
{
    public static void MapAuth(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/register", Register);
        authGroup.MapPost("/login", Login);
        authGroup.MapGet("/protected", Test).RequireAuthorization();
        authGroup.MapGet("/refresh", RefreshToken);
    }

    private static async Task<Ok<Guid>> Register([FromBody] RegisterUserDto dto,
        [FromServices] ILogger<Program> logger,
        [FromServices] IUsersHandler handler,
        CancellationToken ct
    )
    {
        logger.LogInformation("Registering user {Dto}", dto);
        var response = await handler.RegisterUser(dto, ct);
        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<AccessTokenDto>, UnauthorizedHttpResult>> Login(
        [FromBody] LoginUserDto command,
        HttpContext httpContext,
        [FromServices] ILogger<Program> logger,
        [FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        logger.LogInformation("Logging in");
        httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken);
        var response = await handler.LoginUser(command, refreshToken, ct);
        return response.ToAccessTokenDto(httpContext);
    }

    private static async Task<Results<Ok<AccessTokenDto>, UnauthorizedHttpResult>> RefreshToken(HttpContext httpContext,
        [FromServices] IUsersHandler handler, CancellationToken cancellationToken)
    {
        if (!httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken))
        {
            return TypedResults.Unauthorized();
        }

        var response = await handler.RefreshToken(refreshToken, cancellationToken);
        return response.ToAccessTokenDto(httpContext);
    }

    private static string Test()
    {
        return "Hello";
    }

    private static Results<Ok<AccessTokenDto>, UnauthorizedHttpResult> ToAccessTokenDto(this TokensDto? dto,
        HttpContext httpContext)
    {
        if (dto is null) return TypedResults.Unauthorized();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        httpContext.Response.Cookies.Append("refresh-token", dto.RefreshToken, cookieOptions);
        return TypedResults.Ok(new AccessTokenDto(dto.AccessToken));
    }
}