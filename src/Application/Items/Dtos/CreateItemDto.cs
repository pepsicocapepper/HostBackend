using Domain.Common;

namespace Application.Items.Dtos;

public record CreateItemDto(
    string Name,
    string? PosName,
    int? Plu,
    string? Sku,
    string? Description,
    PricingModel PricingModel,
    List<CreateItemPriceDto> Prices,
    List<Guid> ModifierIds,
    List<Guid> RecipeIds
);