using Domain.Entities;

namespace Application.Bills.Dtos;

public record CreateBillDto(
    decimal Amount,
    List<CreateBillItemDto> BillItems,
    DateTime CreatedAt
);