using Application.Menus.Dtos;
using FluentValidation;

namespace Application.Menus.Validators;

internal sealed class CreateMenuDtoValidator : AbstractValidator<CreateMenuDto>
{
    public CreateMenuDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Maximum length of name is 30 characters");
    }
}