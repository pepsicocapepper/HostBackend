using Application.Staffings.Dto;
using FluentValidation;

namespace Application.Staffing.Validators;

internal sealed class RegisterStaffingDtoValidator : AbstractValidator<RegisterStaffingDto>
{
    public RegisterStaffingDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}