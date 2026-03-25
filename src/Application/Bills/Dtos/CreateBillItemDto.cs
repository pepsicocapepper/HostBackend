using Domain.Common;

namespace Application.Bills.Dtos;

public record CreateBillItemDto(
    int ItemId,
    decimal Price,
    Denomination Denomination,
    int Quantity,
    List<CreateBillItemModifierDto> Modifiers
);