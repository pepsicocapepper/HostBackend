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
using Application.Staffings.Dto;

namespace Api.Endpoints;

public static class Staffings
{
    public static void MapStaffing(this WebApplication app)
    {
        var group = app.MapGroup("/staffing");

        // group.MapPost("/", CreateUser).RequireAuthorization();
        group.MapGet("/", GetPaginatedStaffings);
        group.MapGet("/{id}", GetStaffing);
        // group.MapPut("/{id}",EditUser);
        // group.MapDelete("/{id}",DeleteUser);
    }

    private static async Task<Ok<PaginatedData<StaffingDto>>> GetPaginatedStaffings([FromServices] IStaffingHandler handler,
        CancellationToken ct)
    {
        var staffings = await handler.GetPaginatedStaffings(ct);
        return TypedResults.Ok(staffings);
    }

     private static async Task<Results<Ok<StaffingDto>, NotFound>> 
     GetStaffing([FromServices] IStaffingHandler handler,CancellationToken ct,Guid id)
    {
        var staffing = await handler.GetStaffing(id,ct);
        return  staffing is null? TypedResults.NotFound():TypedResults.Ok(staffing);
    }
}