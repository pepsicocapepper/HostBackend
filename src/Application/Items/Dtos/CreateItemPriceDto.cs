using Domain.Common;

namespace Application.Items.Dtos;

public record CreateItemPriceDto(decimal Price, Denomination Denomination);