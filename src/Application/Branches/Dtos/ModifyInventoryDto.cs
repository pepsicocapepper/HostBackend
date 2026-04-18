namespace Application.Branches.Dtos;

public record ModifyInventoryDto(
    List<ModifyInventoryIngredientDto> Ingredients
);