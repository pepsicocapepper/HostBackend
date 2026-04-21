using Application.Bills.Dtos;

namespace Application.Intuit.Dtos;

public record SelectedBillsDto(List<Guid> BillIds);