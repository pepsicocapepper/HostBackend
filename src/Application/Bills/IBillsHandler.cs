using Application.Bills.Dtos;
using Application.Common.Models;

namespace Application.Bills;

public interface IBillsHandler
{
    Task<PaginatedData<BillDto>> GetPaginatedBillsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateBill(CreateBillDto createBillDto, CancellationToken cancellationToken = default);
}