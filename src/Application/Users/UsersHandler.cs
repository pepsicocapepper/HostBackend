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

namespace Application.Users;

public class UsersHandler : IUsersHandler
{   
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    private readonly IValidator<RegisterUserDto> _registerUserValidator;
    private readonly IValidator<MinimalUserPunchTimeDto> _registerUserPunchTimeValidator;

    public UsersHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper mapper,
        IValidator<RegisterUserDto> registerUserValidator,IValidator<MinimalUserPunchTimeDto> registerUserPunchTimeValidator) 
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _mapper = mapper;
        _registerUserValidator = registerUserValidator;
        _registerUserPunchTimeValidator = registerUserPunchTimeValidator;
    }

    public async Task<UserDto?> GetUser(Guid id,CancellationToken cancellationToken)
    {
       return await _dbContext.Users
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

    }

    public async Task<bool> DeleteUser(Guid id,CancellationToken cancellationToken)
    {
        try
        {   

            var user = await _dbContext.Users
                .Where(u=>u.Id==id)
                .FirstOrDefaultAsync(cancellationToken);
                user!.Active=false;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<EditUserDto?> EditUser(Guid id,EditUserDto editUserDto,CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
                        .Where(u=>u.Id==id)
                        .FirstOrDefaultAsync(cancellationToken);
                    
        if (user is null)
        {
            return null;
        }

        user.Name=editUserDto.Name;
        user.Surname=editUserDto.Surname;
        user.Pin=editUserDto.Pin;
        user.Phone=editUserDto.Phone;
        user.JobTitle=editUserDto.JobTitle;
        user.Active=editUserDto.Active;
        user.BranchId=editUserDto.BranchId;
        user.StaffingId=editUserDto.StaffingId;

        await _dbContext.SaveChangesAsync(cancellationToken);


        return editUserDto;
    }

    public async Task<PaginatedData<UserDto>> GetPaginatedUsers(CancellationToken cancellationToken)
    {
         var users = await _dbContext
            .Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return users;
    }
    public async Task<ErrorOr<Guid>>RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var validationResult = _registerUserValidator.Validate(registerUserDto);

        if (!validationResult.IsValid)
        {
            return Error.Validation("BadRegisterUserDto", "Error",
                validationResult.Errors.ToDictionary(x => x.PropertyName, object (x) => x.ErrorMessage));
        }

        var user = new User
        {
            Name = registerUserDto.Name,
            Surname = registerUserDto.Surname,
            Pin = registerUserDto.Pin,
            JobTitle=registerUserDto.JobTitle,
            BranchId = registerUserDto.BranchId,
            Phone=registerUserDto.Phone,
            StaffingId = registerUserDto.StaffingId,
            Active=true
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }


    public async Task<TokensDto?> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Pin == loginUserDto.Pin, cancellationToken);
        return user != null ? await _tokenProvider.GenerateTokensAsync(user, existingRefreshToken) : null;
    }

    public async Task<TokensDto?> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var dbToken =
            await _dbContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken,
                    cancellationToken: cancellationToken);
        if (dbToken is null) throw new InvalidOperationException("Refresh token not found");
        return await _tokenProvider.GenerateTokensAsync(dbToken.User, dbToken.Token);
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
            .ProjectTo<UserPunchTimeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return punches;
    }

    public async Task<UserPunchTimeDto?> GetPunch(int id,CancellationToken cancellationToken)
    {
       return await _dbContext.UserPunchTimes
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserPunchTimeDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

    }

    public async Task<UserPunchTimeDto?> GetPaginatedPunches(int id, CancellationToken cancellationToken)
    {
       return await _dbContext.UserPunchTimes
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserPunchTimeDto>(_mapper.ConfigurationProvider)
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