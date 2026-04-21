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

public class StaffingHandler : IStaffingHandler
{   
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;
    private readonly IValidator<RegisterUserDto> _registerUserValidator;

    public StaffingHandler(IApplicationDbContext dbContext, ITokenProvider tokenProvider, IMapper mapper,
        IValidator<RegisterUserDto> registerUserValidator) 
    {
        _dbContext = dbContext;
        // _tokenProvider = tokenProvider;
        _mapper = mapper;
        _registerUserValidator = registerUserValidator;
    }

    public async Task<PaginatedData<StaffingDto>> GetPaginatedStaffings(CancellationToken cancellationToken)
    {
         var staffings = await _dbContext
            .Staffings
            .ProjectTo<StaffingDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1,10,cancellationToken);
        return staffings;
    }

    public async Task<StaffingDto?> GetStaffing(Guid id, CancellationToken cancellationToken)
    {
       return await _dbContext.Staffings
                        .Where(u=>u.Id==id)
                        .ProjectTo<StaffingDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}