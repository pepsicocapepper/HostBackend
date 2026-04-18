using Application.Branches.Dtos;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using Application.Users.Dtos;
using ErrorOr;

namespace Application.Branches;

public interface IBranchesHandler
{
    public Task<PaginatedData<BranchDto>> GetPaginatedBranches(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<BranchDto>> GetBranchById(Guid id, CancellationToken cancellationToken = default);

    public Task<ErrorOr<PaginatedData<MinimalUserDto>>> GetPaginatedBranchUsers(PaginationQuery paginationQuery,
        Guid branchId, CancellationToken cancellationToken = default);

    public Task<ErrorOr<PaginatedData<BranchIngredientDto>>> GetPaginatedBranchIngredients(
        PaginationQuery paginationQuery,
        Guid branchId, CancellationToken cancellationToken = default);

    public Task<ErrorOr<int>> ModifyInventory(Guid branchId, ModifyInventoryDto dto,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<int>> AddIngredientToBranch(Guid branchId, AddIngredientDto dto,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<PaginatedData<IngredientDto>>> GetPaginatedIngredientsNotInBranch(
        PaginationQuery paginationQuery,
        Guid branchId, CancellationToken cancellationToken = default);

    public Task<ErrorOr<Guid>> CreateBranch(CreateBranchDto branchDto, CancellationToken cancellationToken = default);
}