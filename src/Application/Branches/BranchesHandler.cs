using Application.Branches.Dtos;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;

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

    public async Task<ErrorOr<Guid>> CreateBranch(CreateBranchDto branchDto, CancellationToken cancellationToken = default)
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