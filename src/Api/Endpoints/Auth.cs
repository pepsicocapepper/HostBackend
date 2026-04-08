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
        authGroup.MapGet("/refresh", RefreshToken);
    }

    private static async Task<Results<Ok<Guid>, BadRequest<ProblemDetails>>> Register([FromBody] RegisterUserDto dto,
        [FromServices] ILogger<Program> logger,
        [FromServices] IUsersHandler handler,
        CancellationToken ct
    )
    {
        logger.LogInformation("Registering user {Dto}", dto);
        var response = await handler.RegisterUser(dto, ct);
        if (response.IsError)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Detail = "Error registering user",
            });
        }

        return TypedResults.Ok(response.Value);
    }

    private static async Task<Results<Ok<AccessTokenDto>, NotFound>> Login(
        [FromBody] LoginUserDto command,
        HttpContext httpContext,
        [FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken);
        var response = await handler.LoginUser(command, refreshToken, ct);
        return response.ToAccessTokenDto(httpContext);
    }

    private static async Task<Results<Ok<AccessTokenDto>, NotFound>> RefreshToken(HttpContext httpContext,
        [FromServices] IUsersHandler handler, CancellationToken cancellationToken)
    {
        if (!httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken))
        {
            return TypedResults.NotFound();
        }

        var response = await handler.RefreshToken(refreshToken, cancellationToken);
        return response.ToAccessTokenDto(httpContext);
    }

    private static Results<Ok<AccessTokenDto>, NotFound> ToAccessTokenDto(this TokensDto? dto,
        HttpContext httpContext)
    {
        if (dto is null) return TypedResults.NotFound();

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