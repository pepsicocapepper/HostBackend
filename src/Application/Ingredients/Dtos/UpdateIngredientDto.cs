namespace Application.Ingredients.Dtos;

public record UpdateIngredientDto(int Id, string Name, List<Guid> ProviderIds);