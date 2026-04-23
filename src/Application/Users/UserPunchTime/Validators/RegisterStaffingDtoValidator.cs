using Application.Staffings.Dto;
using Application.UserPunchTime.Dto;
using Application.Users.Commands.RegisterUser;
using FluentValidation;

namespace Application.Users.Validators;

internal sealed class MinimalUserPunchTimeDtoValidator : AbstractValidator<MinimalUserPunchTimeDto>
{
    public MinimalUserPunchTimeDtoValidator()
    {
        //validacion se activaba con falso de booleano
        // RuleFor(x => x.IsEntrance)
        //     .NotEmpty().WithMessage("Entrance must be defined");

    }
}