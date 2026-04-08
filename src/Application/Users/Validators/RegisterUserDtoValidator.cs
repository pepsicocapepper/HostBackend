using Application.Users.Commands.RegisterUser;
using FluentValidation;

namespace Application.Users.Validators;

internal sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required");

        RuleFor(x => x.Pin)
            .NotEmpty().WithMessage("Pin required")
            .Matches("^[0-9]{4}");
    }
}