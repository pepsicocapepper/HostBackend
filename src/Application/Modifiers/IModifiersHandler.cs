using Application.Common.Models;
using Application.Modifiers.Dtos;
using ErrorOr;

namespace Application.Modifiers;

public interface IModifiersHandler
{
    public Task<PaginatedData<ModifierElementDto>> GetPaginatedModifierElements(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);

    public Task<PaginatedData<ModifierGroupWithElementsDto>> GetPaginatedModifierGroups(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<PaginatedData<ModifierGroupDto>>> GetPaginatedModGroupsNotInModElement(
        PaginationQuery paginationQuery, Guid modId, CancellationToken cancellationToken = default);

    public Task<ErrorOr<bool>> CreateModifierGroup(CreateModifierGroupDto dto,
        CancellationToken cancellationToken = default);

    public Task<ErrorOr<bool>> CreateModifierElement(CreateModifierElementDto dto,
        CancellationToken cancellationToken = default);
}