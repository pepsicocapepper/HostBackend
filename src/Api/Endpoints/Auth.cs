using Api.Common.Extensions;
using Application.Common.Dtos;
using Application.Users;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Auth
{
    public static void MapAuth(this WebApplication app)
    {
        var group = app.MapGroup("/auth");

        group.MapGet("/me", GetSelfInfo).RequireAuthorization();
        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/logout", Logout);
        group.MapGet("/refresh", RefreshToken);
    }

    private static async Task<Results<Ok<MinimalUserDto>, UnauthorizedHttpResult>> GetSelfInfo(
        [FromServices] IUsersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetSelfInfo(ct);

        if (result.IsError)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(result.Value);
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

    private static async Task<Results<Ok<AccessTokenDto>, NotFound<ProblemDetails>>> Login(
        [FromBody] LoginUserDto command, [FromServices] IUsersHandler handler,
        HttpContext httpContext, CancellationToken ct)
    {
        httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken);
        var response = await handler.LoginUser(command, refreshToken, ct);

        if (response.IsError)
        {
            return TypedResults.NotFound(response.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(response.Value.ToAccessTokenDto(httpContext));
    }

    private static async Task<NoContent> Logout([FromServices] IUsersHandler handler, HttpContext httpContext,
        CancellationToken ct)
    {
        httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken);

        if (refreshToken != null)
        {
            await handler.LogoutUser(refreshToken, ct);
        }

        httpContext.Response.Cookies.Delete("refresh-token");

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<AccessTokenDto>, NotFound<ProblemDetails>, BadRequest>> RefreshToken(
        HttpContext httpContext,
        [FromServices] IUsersHandler handler, CancellationToken cancellationToken)
    {
        if (!httpContext.Request.Cookies.TryGetValue("refresh-token", out var refreshToken))
        {
            return TypedResults.BadRequest();
        }

        var result = await handler.RefreshToken(refreshToken, cancellationToken);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value.ToAccessTokenDto(httpContext));
    }

    private static AccessTokenDto ToAccessTokenDto(this TokensDto dto,
        HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        httpContext.Response.Cookies.Append("refresh-token", dto.RefreshToken, cookieOptions);
        return new AccessTokenDto(dto.AccessToken);
    }
}