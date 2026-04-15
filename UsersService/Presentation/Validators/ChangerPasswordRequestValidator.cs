using FluentValidation;
using UsersService.Presentation.DTO.ChangePassword;

namespace UsersService.Presentation.Validators;

public class ChangerPasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangerPasswordRequestValidator()
    {
        RuleFor(request => request.CurrentPassword).NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one letter and one digit");
        RuleFor(request => request.NewPassword).NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one letter and one digit");
    }
}