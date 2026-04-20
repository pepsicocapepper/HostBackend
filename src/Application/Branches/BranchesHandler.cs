using Application.Branches.Dtos;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Branches;

internal class BranchesHandler : IBranchesHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public BranchesHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<BranchDto>> GetPaginatedBranches(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Branches
            .ProjectTo<BranchDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<List<BranchDto>> GetAllBranches(CancellationToken cancellationToken = default)
    {
        return await _dbContext
                .Branches
                .AsNoTracking() 
                .ProjectTo<BranchDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<BranchDto>> GetBranchById(Guid id, CancellationToken cancellationToken = default)
    {
        var branch = await _dbContext
            .Branches
            .FindAsync([id], cancellationToken);

        if (branch == null)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        return _mapper.Map<BranchDto>(branch);
    }

    public async Task<ErrorOr<PaginatedData<MinimalUserDto>>> GetPaginatedBranchUsers(PaginationQuery paginationQuery,
        Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var branch = await _dbContext
            .Branches
            .FindAsync([branchId], cancellationToken);

        if (branch == null)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        return await _dbContext
            .Users
            .Where(u => u.BranchId == branch.Id)
            .ProjectTo<MinimalUserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<Guid>> CreateBranch(CreateBranchDto branchDto,
        CancellationToken cancellationToken = default)
    {
        var branch = new Branch
        {
            AddressLine1 = branchDto.AddressLine1,
            AddressLine2 = branchDto.AddressLine2,
            ZipCode = branchDto.ZipCode,
            Locality = branchDto.Locality,
            AdministrativeArea = branchDto.AdministrativeArea,
            Country = branchDto.Country
        };

        await _dbContext.Branches.AddAsync(branch, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return branch.Id;
    }
}