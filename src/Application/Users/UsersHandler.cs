using Application.Common.Abstractions;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Users;

public class UsersHandler : IUsersHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserContext _userContext;
    private readonly IValidator<RegisterUserDto> _registerUserValidator;
    private readonly IMapper _mapper;

    public UsersHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider,
        IValidator<RegisterUserDto> registerUserValidator, IUserContext userContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _registerUserValidator = registerUserValidator;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<ErrorOr<MinimalUserDto>> GetSelfInfo(CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([_userContext.UserId!.Value], cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User.NotFound");
        }

        return _mapper.Map<MinimalUserDto>(user);
    }

    public async Task<ErrorOr<Guid>> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
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
            BranchId = registerUserDto.BranchId
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }

    public async Task<ErrorOr<TokensDto>> LoginUser(LoginUserDto loginUserDto, string? existingRefreshToken,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Pin == loginUserDto.Pin, cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User.InvalidLogin");
        }

        return await _tokenProvider.GenerateTokensAsync(user, existingRefreshToken, cancellationToken);
    }

    public async Task LogoutUser(string refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext
            .RefreshTokens
            .Where(u => u.Token == refreshToken)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<ErrorOr<TokensDto>> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var dbToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);

        if (dbToken == null)
        {
            return Error.NotFound("User.RefreshTokenNotFound");
        }

        return await _tokenProvider.GenerateTokensAsync(dbToken.User, dbToken.Token);
    }
}