using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Staffing.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;

namespace Application.Staffing;

public class StaffingHandler : IStaffingHandler
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _dbContext;

    public StaffingHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<StaffingDto>> GetPaginatedStaffings(PaginationQuery query,
        CancellationToken cancellationToken)
    {
        return await _dbContext
            .Staffings
            .ProjectTo<StaffingDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<ErrorOr<StaffingDto>> GetStaffing(Guid id, CancellationToken cancellationToken)
    {
        var staffing = await _dbContext.Staffings
            .FindAsync([id], cancellationToken);

        if (staffing == null)
        {
            return Error.NotFound(StaffingErrorCodes.NotFound);
        }

        return _mapper.Map<StaffingDto>(staffing);
    }
}