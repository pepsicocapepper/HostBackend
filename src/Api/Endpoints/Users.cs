using Application.Common.Models;
using Application.Items;
using Application.Items.Dtos;
using Application.Users;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace Api.Endpoints;

public static class Users
{
    public static void MapUsers(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapPost("/", CreateUser).RequireAuthorization();
        group.MapGet("/", GetPaginatedUsers).WithName("GetPaginatedUsers");
        group.MapGet("/{id}", GetUser).WithName("GetUser");
        group.MapPut("/{id}",EditUser);
        group.MapDelete("/{id}",DeleteUser);
    }

private static async Task<Results<CreatedAtRoute<UserDto>, BadRequest>> CreateUser(
    [FromServices] IUsersHandler handler,
    [FromBody] RegisterUserDto dto,
    CancellationToken ct)
{
    var result = await handler.RegisterUser(dto, ct);

    if (!result.IsError)
    {
        Guid id = result.Value;
        
        // Assuming GetUser returns a UserDto or ErrorOr<UserDto>
        var response = await handler.GetUser(id, ct); 
        
        // You'll need to handle if GetUser fails, but for now:
        return TypedResults.CreatedAtRoute(response, "GetUser", new { id });
    }
    else
    {
        // To return a "Not OK" result with details:
        return TypedResults.BadRequest();
    }
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

    private static async Task<Results<Ok<EditUserDto?>, NotFound>> 
    EditUser([FromServices] IUsersHandler handler,Guid id,EditUserDto userDto,CancellationToken ct)
    {
        var user = await handler.EditUser(id,userDto,ct);
        return  user is null? TypedResults.NotFound():TypedResults.Ok(user)!;
    }

    private static async Task<Results<Ok, InternalServerError>>  
    DeleteUser([FromServices] IUsersHandler handler, Guid id,CancellationToken ct)
    {   
        var delUser = await handler.DeleteUser(id,ct);
        return  delUser? TypedResults.Ok():TypedResults.InternalServerError();
    }
}