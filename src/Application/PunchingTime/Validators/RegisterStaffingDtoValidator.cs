using Application.Staffings.Dto;
using Application.Users.Commands.RegisterUser;
using FluentValidation;

namespace Application.Users.Validators;

internal sealed class RegisterStaffingDtoValidator : AbstractValidator<RegisterStaffingDto>
{
    public RegisterStaffingDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

    }
}