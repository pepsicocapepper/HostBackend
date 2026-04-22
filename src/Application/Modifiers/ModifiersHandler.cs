using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Modifiers.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;

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

    public async Task<PaginatedData<ModifierElementDto>> GetPaginatedModifiers(PaginationQuery paginationQuery,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .ModifierElements
            .ProjectTo<ModifierElementDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }
}