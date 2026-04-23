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
using Application.UserPunchTime.Dto;

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

        var punchGroup = app.MapGroup("/users/punch");

        punchGroup.MapPost("/{id}", Punch);
        punchGroup.MapGet("/", GetPaginatedPunches).WithName("GetPaginatedPunches");
        punchGroup.MapGet("/{id}", GetPunch).WithName("GetPunch");
        punchGroup.MapPut("/{id}",EditPunch);
        punchGroup.MapDelete("/{id}",DeletePunch);
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

    private static async Task<Results<CreatedAtRoute<UserPunchTimeDto>, BadRequest>> Punch(
    [FromServices] IUsersHandler handler,
    [FromBody] RegisterUserPunchTimeDto dto,
    CancellationToken ct,Guid id)
{
    var result = await handler.Punch(id,dto, ct);

    if (!result.IsError)
    {
        int punchId = result.Value;
        
        // Assuming GetUser returns a UserDto or ErrorOr<UserDto>
        var response = await handler.GetPunch(punchId, ct); 
        
        // You'll need to handle if GetUser fails, but for now:
        return TypedResults.CreatedAtRoute(response, "GetPunch", new { punchId });
    }
    else
    {
        // To return a "Not OK" result with details:
        return TypedResults.BadRequest();
    }
}

    private static async Task<Ok<PaginatedData<UserPunchTimeDto>>> GetPaginatedPunches([FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var punches = await handler.GetPaginatedPunches(ct);
        return TypedResults.Ok(punches);
    }

     private static async Task<Results<Ok<UserPunchTimeDto>, NotFound>> 
     GetPunch([FromServices] IUsersHandler handler,CancellationToken ct,int id)
    {
        var punch = await handler.GetPunch(id,ct);
        return  punch is null? TypedResults.NotFound():TypedResults.Ok(punch);
    }

    private static async Task<Results<Ok<MinimalUserPunchTimeDto?>, NotFound>> 
    EditPunch([FromServices] IUsersHandler handler,int id,EditUserPunchTimeDto userPunchTimeDto,CancellationToken ct)
    {
        var punch = await handler.EditPunch(id,userPunchTimeDto,ct);
        return  punch is null? TypedResults.NotFound():TypedResults.Ok(punch)!;
    }

    private static async Task<Results<Ok, InternalServerError>>  
    DeletePunch([FromServices] IUsersHandler handler, int id,CancellationToken ct)
    {   
        var delPunch = await handler.DeletePunch(id,ct);
        return  delPunch? TypedResults.Ok():TypedResults.InternalServerError();
    }
}