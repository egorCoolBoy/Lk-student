using FluentValidation;
using UsersService.Presentation.DTO;
using Contracts;

namespace UsersService.Presentation.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one letter and one digit");
        
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid role");
    }
}