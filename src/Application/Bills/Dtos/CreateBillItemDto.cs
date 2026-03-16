namespace Application.Bills.Dtos;

public record CreateBillItemDto(
    int ItemId,
    int Quantity
);