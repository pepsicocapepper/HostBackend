using Application.Common.Abstractions;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common.Models;
using Application.Common.Mappings;
using Application.Users.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Reflection.Metadata.Ecma335;
using ErrorOr;
using FluentValidation;
using Application.UserPunchTime.Dto;

namespace Application.PunchingTime;

public class UserPunchTimeHandler : IUserPunchTimeHandler
{   
    private readonly IMapper _punchMapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly IValidator<MinimalUserPunchTimeDto> _registerUserPunchTimeValidator;

    public UserPunchTimeHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper punchMapper,
        IValidator<MinimalUserPunchTimeDto> registerPunchingTimeValidator) 
    {
        _dbContext = dbContext;
        // _tokenProvider = tokenProvider;
        _punchMapper = punchMapper;
        _registerUserPunchTimeValidator = registerPunchingTimeValidator;
    }
    public async Task<ErrorOr<int>>Punch(Guid id, MinimalUserPunchTimeDto minPunchTimeDto, CancellationToken cancellationToken)
    {
        var validationResult = _registerUserPunchTimeValidator.Validate(minPunchTimeDto);

        if (!validationResult.IsValid)
        {
            return Error.Validation("MinimalUserPunchTimeDto", "Error",
                validationResult.Errors.ToDictionary(x => x.PropertyName, object (x) => x.ErrorMessage));
        }

        var punch = new Domain.Entities.UserPunchTime
        {
            IsEntrance=minPunchTimeDto.IsEntrance,
            UserId=id
        };

        await _dbContext.UserPunchTimes.AddAsync(punch, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return punch.Id;
    }

    public async Task<PaginatedData<UserPunchTimeDto>> GetPaginatedPunches(CancellationToken cancellationToken)
    {
         var punches = await _dbContext
            .UserPunchTimes
            .ProjectTo<UserPunchTimeDto>(_punchMapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return punches;
    }

    public async Task<UserPunchTimeDto?> GetPunch(int id,CancellationToken cancellationToken)
    {
       return await _dbContext.UserPunchTimes
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserPunchTimeDto>(_punchMapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

    }

    public async Task<UserPunchTimeDto?> GetPaginatedPunches(int id, CancellationToken cancellationToken)
    {
       return await _dbContext.UserPunchTimes
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserPunchTimeDto>(_punchMapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<EditUserPunchTimeDto?> EditPunch(int id,EditUserPunchTimeDto editPunchDto,CancellationToken cancellationToken)
    {
        var punch = await _dbContext.UserPunchTimes
                .Where(u=>u.Id==id)
                .FirstOrDefaultAsync(cancellationToken);
                    
        if (punch is null)
        {
            return null;
        }

        punch.IsEntrance=editPunchDto.IsEntrance;
        punch.UserId=editPunchDto.UserId;

        await _dbContext.SaveChangesAsync(cancellationToken);


        return editPunchDto;
    }
    public async Task<bool> DeletePunch(int id,CancellationToken cancellationToken)
    {
        try
        {   

            var punch = await _dbContext.UserPunchTimes
                .Where(u=>u.Id==id)
                .FirstOrDefaultAsync(cancellationToken);
            if (punch != null)
            {
                _dbContext.UserPunchTimes.Remove(punch);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }
}