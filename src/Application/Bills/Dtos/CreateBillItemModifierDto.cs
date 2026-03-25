using Domain.Common;

namespace Application.Bills.Dtos;

public class CreateBillItemModifierDto
{
    public Guid ModifierId { get; set; }
    public decimal Price { get; set; }
    public Denomination Denomination { get; set; }
}