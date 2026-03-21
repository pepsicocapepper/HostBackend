using Domain.Common;

namespace Application.Items.Dtos;

public record CreateItemPriceDto(ulong Price, Denomination Denomination, short NegativeExponent);