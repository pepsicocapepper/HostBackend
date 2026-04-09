using Application.Ingredients.Dtos;
using FluentValidation;

namespace Application.Ingredients.Validators;

public class CreateIngredientDtoValidator : AbstractValidator<CreateIngredientDto>
{
    public CreateIngredientDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Ingredient name is required.");
    }
}