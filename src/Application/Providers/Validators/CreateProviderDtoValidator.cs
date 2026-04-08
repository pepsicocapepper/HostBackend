using Application.Providers.Dtos;
using FluentValidation;

namespace Application.Providers.Validators;

public class CreateProviderDtoValidator : AbstractValidator<CreateProviderDto>
{
    public CreateProviderDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("{PropertyName} is invalid.");
    }
}