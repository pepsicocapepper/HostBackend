using Domain.Entities;
using FluentValidation;

namespace Application.Users.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Pin).NotEmpty().Matches("^[0-9]{4}");
    }
}