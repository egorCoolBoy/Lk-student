using FluentValidation;
using UsersService.Domain;
using UsersService.Presentation.DTO;

namespace UsersService.Presentation.Validators;

public class RegisterManagerRequestValidator : AbstractValidator<RegisterManagerRequest>
{
    public RegisterManagerRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);


    }
}