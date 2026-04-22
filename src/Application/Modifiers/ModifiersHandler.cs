using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Modifiers.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Modifiers;

internal class ModifiersHandler : IModifiersHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ModifiersHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ModifierElementDto>> GetPaginatedModifierElements(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .ModifierElements
            .ProjectTo<ModifierElementDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<PaginatedData<ModifierGroupWithElementsDto>> GetPaginatedModifierGroups(
        PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .ModifierGroups
            .ProjectTo<ModifierGroupWithElementsDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<PaginatedData<ModifierGroupDto>>> GetPaginatedModGroupsNotInModElement(
        PaginationQuery paginationQuery, Guid modId,
        CancellationToken cancellationToken = default)
    {
        var modExists = await _dbContext.ModifierElements.AnyAsync(m => m.Id == modId, cancellationToken);

        if (!modExists)
        {
            return Error.NotFound(ModifierErrorCodes.Element.NotFound);
        }

        return await _dbContext
            .ModifierGroups
            .Where(modifierGroup =>
                modifierGroup.ModifierGroupElements.Any(ip =>
                    ip.ModifierGroupId == modifierGroup.Id && ip.ModifierElementId == modId))
            .ProjectTo<ModifierGroupDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<bool>> CreateModifierGroup(CreateModifierGroupDto dto,
        CancellationToken cancellationToken = default)
    {
        var modGroup = new ModifierGroup
        {
            Name = dto.Name,
        };

        await _dbContext.ModifierGroups.AddAsync(modGroup, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> CreateModifierElement(CreateModifierElementDto dto,
        CancellationToken cancellationToken = default)
    {
        var modElement = new ModifierElement
        {
            Name = dto.Name,
            Price = dto.Price,
            Denomination = Denomination.Usd
        };

        await _dbContext.ModifierElements.AddAsync(modElement, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}