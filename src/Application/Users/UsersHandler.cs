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

namespace Application.Users;

public class UsersHandler : IUsersHandler
{   
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    private readonly IValidator<RegisterUserDto> _registerUserValidator;

    public UsersHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper mapper,
        IValidator<RegisterUserDto> registerUserValidator) 
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _mapper = mapper;
        _registerUserValidator = registerUserValidator;
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
            await _dbContext.Users.Where(user=>user.Id==id)
                                .ExecuteDeleteAsync(cancellationToken);
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
            BranchId = registerUserDto.BranchId
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

}