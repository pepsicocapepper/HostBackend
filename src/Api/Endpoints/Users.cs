using Application.Common.Models;
using Application.Items;
using Application.Items.Dtos;
using Application.Users;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class Users
{
    public static void MapUsers(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapPost("/", CreateUser).RequireAuthorization();
        group.MapGet("/", GetPaginatedUsers).WithName("GetPaginatedUsers");
    }

    private static async Task<CreatedAtRoute<Guid>> CreateUser([FromServices] IUsersHandler handler,
        [FromBody] RegisterUserDto dto,
        CancellationToken ct)
    {
        var id = await handler.RegisterUser(dto, ct);
        return TypedResults.CreatedAtRoute(id, "GetPaginatedUsers", new { });
    }

    private static async Task<Ok<PaginatedData<UserDto>>> GetPaginatedUsers([FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var users = await handler.GetPaginatedUsers(ct);
        return TypedResults.Ok(users);
    }

}