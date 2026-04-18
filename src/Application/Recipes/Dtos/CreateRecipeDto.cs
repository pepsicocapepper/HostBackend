namespace Application.Recipes.Dtos;

public record CreateRecipeDto(string Name, List<CreateRecipeIngredientDto> Ingredients, List<string> Steps);