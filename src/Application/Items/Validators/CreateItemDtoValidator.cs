using Application.Items.Dtos;
using FluentValidation;

namespace Application.Items.Validators;

internal sealed class CreateItemDtoValidator : AbstractValidator<CreateItemDto>
{
    public CreateItemDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithName("EmptyName").WithMessage("Name is required");

        RuleFor(x => x.Prices)
            .NotNull().NotEmpty().WithName("EmptyPrice").WithMessage("Price is required");
    }
}