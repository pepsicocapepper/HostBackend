using Application.Staffings.Dto;
using Application.UserPunchTime.Dto;
using Application.Users.Commands.RegisterUser;
using Application.Users.Dtos;
using FluentValidation;

namespace Application.Users.Validators;

internal sealed class MinimalUserPunchTimeDtoValidator : AbstractValidator<RegisterUserPunchTimeDto>
{
    public MinimalUserPunchTimeDtoValidator()
    {
        // validacion se activaba con falso de booleano
        RuleFor(x => x.Pin)
            .NotEmpty().WithMessage("Pin must be defined");

    }
}