namespace Application.Recipes.Dtos;

public record UpdateRecipeDto(Guid Id, string Name, List<CreateRecipeIngredientDto> Ingredients, List<string> Steps);
