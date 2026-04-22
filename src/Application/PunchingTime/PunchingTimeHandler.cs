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
using Application.Staffings.Dto;

namespace Application.Users;

public class PunchingTimeHandler : IPunchingTimeHandler
{   
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly IValidator<RegisterPunchingTimeDto> _registerPunchingTimeValidator;

    public PunchingTimeHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper mapper,
        IValidator<RegisterPunchingTimeDto> registerPunchingTimeValidator) 
    {
        _dbContext = dbContext;
        // _tokenProvider = tokenProvider;
        _mapper = mapper;
        _registerPunchingTimeValidator = registerPunchingTimeValidator;
    }
    public async Task<ErrorOr<Guid>>Punch(RegisterPunchingTimeDto registerPunchingTimeDto, CancellationToken cancellationToken)
    {
        var validationResult = _registerPunchingTimeValidator.Validate(registerPunchingTimeDto);

        if (!validationResult.IsValid)
        {
            return Error.Validation("BadRegisterPunchingTimeDto", "Error",
                validationResult.Errors.ToDictionary(x => x.PropertyName, object (x) => x.ErrorMessage));
        }

        var punch = new PunchingTime
        {
            InOrOut=registerPunchingTimeDto.InOrOut,
            UserId=registerPunchingTimeDto.UserId,
            Active=true
        };

        await _dbContext.PunchingTimes.AddAsync(punch, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return punch.Id;
    }

    public async Task<PaginatedData<PunchingTimeDto>> GetPaginatedPunches(CancellationToken cancellationToken)
    {
         var punches = await _dbContext
            .PunchingTimes
            .ProjectTo<PunchingTimeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return punches;
    }

    public async Task<PunchingTimeDto?> GetPaginatedPunches(Guid id, CancellationToken cancellationToken)
    {
       return await _dbContext.PunchingTimes
                        .Where(u=>u.Id==id)
                        .ProjectTo<PunchingTimeDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<EditPunchingTimeDto?> EditPunch(Guid id,EditPunchingTimeDto editPunchDto,CancellationToken cancellationToken)
    {
        var punch = await _dbContext.PunchingTimes
                .Where(u=>u.Id==id)
                .FirstOrDefaultAsync(cancellationToken);
                    
        if (punch is null)
        {
            return null;
        }

        punch.InOrOut=editPunchDto.InOrOut;
        punch.UserId=editPunchDto.UserId;
        punch.Active=editPunchDto.Active;

        await _dbContext.SaveChangesAsync(cancellationToken);


        return editPunchDto;
    }
    public async Task<bool> DeletePunch(Guid id,CancellationToken cancellationToken)
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
}