using Application.Branches.Dtos;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Ingredients.Dtos;
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
    private readonly IUserContext _userContext;

    public BranchesHandler(IApplicationDbContext dbContext, IMapper mapper, IUserContext userContext)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<PaginatedData<BranchDto>> GetPaginatedBranches(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Branches
            .ProjectTo<BranchDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
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
        var branchExists = await _dbContext
            .Branches
            .AnyAsync(b => b.Id == branchId, cancellationToken);

        if (!branchExists)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        return await _dbContext
            .Users
            .Where(u => u.BranchId == branchId)
            .ProjectTo<MinimalUserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<PaginatedData<BranchIngredientDto>>> GetPaginatedBranchIngredients(
        PaginationQuery paginationQuery,
        Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var branchExists = await _dbContext
            .Branches
            .AnyAsync(b => b.Id == branchId, cancellationToken);

        if (!branchExists)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        return await _dbContext
            .BranchIngredients
            .Where(u => u.BranchId == branchId)
            .ProjectTo<BranchIngredientDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<int>> ModifyInventory(Guid branchId, ModifyInventoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var branchExists = await _dbContext
            .Branches
            .AnyAsync(b => b.Id == branchId, cancellationToken);

        if (!branchExists)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        var ingredientIds = dto.Ingredients.Select(i => i.Id).ToList();

        var existingInventoryItems = await _dbContext.BranchIngredients
            .Where(b => b.BranchId == branchId && ingredientIds.Contains(b.IngredientId))
            .ToListAsync(cancellationToken);

        if (existingInventoryItems.Count != ingredientIds.Count)
        {
            return Error.Conflict(BranchErrorCodes.InvalidIngredients);
        }

        foreach (var dtoIngredient in dto.Ingredients)
        {
            var inventoryItem = existingInventoryItems
                .First(e => e.IngredientId == dtoIngredient.Id);

            var branchInventoryHistory = new BranchInventoryHistory
            {
                PreviousQuantity = inventoryItem.Quantity,
                NewQuantity = dtoIngredient.Quantity,
                Action = dtoIngredient.Action,
                IngredientId = inventoryItem.IngredientId,
                BranchId = branchId,
                UserId = _userContext.UserId!.Value
            };

            await _dbContext
                .BranchInventoryHistories
                .AddAsync(branchInventoryHistory, cancellationToken);

            inventoryItem.Quantity = dtoIngredient.Quantity;
        }

        var rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsAffected;
    }

    public async Task<ErrorOr<int>> AddIngredientToBranch(Guid branchId, AddIngredientDto dto,
        CancellationToken cancellationToken = default)
    {
        var branchExists = await _dbContext
            .Branches
            .AnyAsync(b => b.Id == branchId, cancellationToken);

        if (!branchExists)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        await _dbContext.BranchIngredients.AddAsync(new BranchIngredient
        {
            IngredientId = dto.Id,
            BranchId = branchId,
            Quantity = dto.Quantity,
            Unit = dto.Unit
        }, cancellationToken);

        var rowsAffected = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsAffected;
    }

    public async Task<ErrorOr<PaginatedData<IngredientDto>>> GetPaginatedIngredientsNotInBranch(
        PaginationQuery paginationQuery, Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var branchExists = await _dbContext
            .Branches
            .AnyAsync(b => b.Id == branchId, cancellationToken);

        if (!branchExists)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        return await _dbContext
            .Ingredients
            .Where(t => !_dbContext.BranchIngredients.Any(bi => bi.IngredientId == t.Id && bi.BranchId == branchId))
            .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider)
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

    public async Task<ErrorOr<bool>> UpdateBranch(UpdateBranchDto dto, CancellationToken cancellationToken = default)
    {
        var branch = await _dbContext.Branches.FindAsync([dto.Id], cancellationToken);

        if (branch == null)
        {
            return Error.NotFound(BranchErrorCodes.NotFound);
        }

        branch.AddressLine1 = dto.AddressLine1;
        branch.AddressLine2 = dto.AddressLine2;
        branch.AdministrativeArea = dto.AdministrativeArea;
        branch.Country = dto.Country;
        branch.ZipCode = dto.ZipCode;
        branch.Locality = dto.Locality;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}