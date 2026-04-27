using Api.Common.Extensions;
using Application.Common.Models;
using Application.Users;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Application.UserPunchTime.Dto;
using Application.Users.Dtos;
using Created = Microsoft.AspNetCore.Http.HttpResults.Created;

namespace Api.Endpoints;

public static class Users
{
    private const string PaginatedUsersRoute = "GetPaginatedUsers";
    private const string GetUserRoute = "GetUser";
    private const string PaginatedPunchesRoute = "GetPaginatedPunches";
    private const string GetPunchRoute = "GetPunch";

    public static void MapUsers(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapPost("/", CreateUser).RequireAuthorization();
        group.MapGet("/", GetPaginatedUsers).WithName(PaginatedUsersRoute);
        group.MapGet("/{id:guid}", GetUser).WithName(GetUserRoute);
        group.MapPut("/{id:guid}", UpdateUser);
        group.MapDelete("/{id:guid}", DeleteUser);

        var punchGroup = app.MapGroup("/users/punch");

        punchGroup.MapGet("/", GetPaginatedPunches).WithName(PaginatedPunchesRoute);
        punchGroup.MapGet("/{id:int}", GetPunch).WithName(GetPunchRoute);
        punchGroup.MapPost("/", Punch);
        punchGroup.MapPut("/{id:int}",UpdatePunch);
        punchGroup.MapDelete("/{id:int}", DeletePunch);
    }

    private static async Task<Results<Created, BadRequest<ProblemDetails>>> CreateUser(
        [FromServices] IUsersHandler handler,
        [FromBody] RegisterUserDto dto,
        CancellationToken ct)
    {
        var result = await handler.RegisterUser(dto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }

    private static async Task<Ok<PaginatedData<UserDto>>> GetPaginatedUsers([FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var users = await handler.GetPaginatedUsers(ct);
        return TypedResults.Ok(users);
    }

    private static async Task<Results<Ok<UserDto>, NotFound<ProblemDetails>>>
        GetUser(Guid id, [FromServices] IUsersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetUser(id, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<Ok, NotFound<ProblemDetails>>>
        UpdateUser(Guid id, [FromServices] IUsersHandler handler, EditUserDto userDto, CancellationToken ct)
    {
        var result = await handler.UpdateUser(id, userDto, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok();
    }

    private static async Task<NoContent> DeleteUser(Guid id, [FromServices] IUsersHandler handler, CancellationToken ct)
    {
        await handler.DeleteUser(id, ct);

        return TypedResults.NoContent();
    }

    private static async Task<Results<Created, BadRequest<ProblemDetails>>> Punch(
        [FromBody] RegisterUserPunchTimeDto dto,
        [FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Punch(dto, ct);

        if (result.IsError)
        {
            return TypedResults.BadRequest(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Created();
    }

    private static async Task<Ok<PaginatedData<UserPunchTimeDto>>> GetPaginatedPunches(
        int? pageNumber, int? pageSize, [FromServices] IUsersHandler handler,
        CancellationToken ct)
    {
        var punches = await handler.GetPaginatedPunches(new PaginationQuery(pageNumber, pageSize), ct);
        return TypedResults.Ok(punches);
    }

    private static async Task<Results<Ok<UserPunchTimeDto>, NotFound<ProblemDetails>>> GetPunch(int id,
        [FromServices] IUsersHandler handler, CancellationToken ct)
    {
        var result = await handler.GetPunch(id, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok(result.Value);
    }

    private static async Task<NoContent>
        DeletePunch([FromServices] IUsersHandler handler, int id, CancellationToken ct)
    {
        await handler.DeletePunch(id, ct);

        return TypedResults.NoContent();
    }

        private static async Task<Results<Ok, NotFound<ProblemDetails>>>
        UpdatePunch(int id, [FromServices] IUsersHandler handler, EditUserPunchTimeDto userPunchDto, CancellationToken ct)
    {
        var result = await handler.UpdatePunch(id, userPunchDto, ct);

        if (result.IsError)
        {
            return TypedResults.NotFound(result.FirstError.ToProblemDetails());
        }

        return TypedResults.Ok();
    }
} 

