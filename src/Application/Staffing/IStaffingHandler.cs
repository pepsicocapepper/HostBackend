using Application.Common.Models;
using Application.Staffing.Dtos;
using ErrorOr;

namespace Application.Staffing;

public interface IStaffingHandler
{
    Task<PaginatedData<StaffingDto>> GetPaginatedStaffings(PaginationQuery query,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<StaffingDto>> GetStaffing(Guid id, CancellationToken cancellationToken = default);
}