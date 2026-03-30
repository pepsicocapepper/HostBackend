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

namespace Application.Users;

public class UsersHandler : IUsersHandler
{   
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public UsersHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper mapper) 
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _mapper = mapper;
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

    public async Task<UserDto?> EditUser(Guid id,UserDto userDto,CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
                        .Where(u=>u.Id==id)
                        .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                    
        if (user is null)
        {
            return null;
        }

        user.Name=userDto.Name;
        user.Surname=userDto.Surname;
        user.Pin=userDto.Pin;
        user.Phone=userDto.Phone;
        user.JobTitle=userDto.JobTitle;
        user.Active=userDto.Active;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return userDto;
    }

    public async Task<PaginatedData<UserDto>> GetPaginatedUsers(CancellationToken cancellationToken)
    {
         var users = await _dbContext
            .Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return users;
    }
    public async Task<Guid> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = registerUserDto.Name,
            Surname = registerUserDto.Surname,
            Pin = registerUserDto.Pin,
            Phone = registerUserDto.Phone,
            JobTitle = registerUserDto.JobTitle,
            Active = registerUserDto.Active
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