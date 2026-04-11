using Application.Branches.Dtos;
using Application.Common.Models;
using ErrorOr;

namespace Application.Branches;

public interface IBranchesHandler
{
    public Task<PaginatedData<BranchDto>> GetPaginatedBranches(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);
    
    public Task<ErrorOr<Guid>> CreateBranch(CreateBranchDto branchDto, CancellationToken cancellationToken = default);
}