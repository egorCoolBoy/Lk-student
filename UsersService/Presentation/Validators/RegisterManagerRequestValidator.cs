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
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one letter and one digit");
        
    }
}