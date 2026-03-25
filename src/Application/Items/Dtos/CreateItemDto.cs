namespace Application.Items.Dtos;

public record CreateItemDto(string Name, List<CreateItemPriceDto> Prices);