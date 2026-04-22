using Application.Common.Models;
using Application.Modifiers.Dtos;

namespace Application.Modifiers;

public interface IModifiersHandler
{
    public Task<PaginatedData<ModifierElementDto>> GetPaginatedModifiers(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default);
}