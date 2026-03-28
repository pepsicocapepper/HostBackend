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
        group.MapGet("/{id}", GetUser).WithName("GetUser");
    }

    private static async Task<CreatedAtRoute<UserDto>> CreateUser([FromServices] IUsersHandler handler,
        [FromBody] RegisterUserDto dto,
        CancellationToken ct)
    {
        var id = await handler.RegisterUser(dto, ct);
        var response = await handler.GetUser(id,ct);  
        return TypedResults.CreatedAtRoute(response,"GetUser",new{id});
        
    }

    private static async Task<Ok<PaginatedData<UserDto>>> GetPaginatedUsers([FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var users = await handler.GetPaginatedUsers(ct);
        return TypedResults.Ok(users);
    }

     private static async Task<Results<Ok<UserDto>, NotFound>> 
     GetUser([FromServices] IUsersHandler handler,CancellationToken ct,Guid id)
    {
        var user = await handler.GetUser(id,ct);
        return  user is null? TypedResults.NotFound():TypedResults.Ok(user);
    }

}