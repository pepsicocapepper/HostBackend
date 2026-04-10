namespace Application.Ingredients.Dtos;

public record CreateIngredientDto(string Name, List<Guid> ProviderIds);