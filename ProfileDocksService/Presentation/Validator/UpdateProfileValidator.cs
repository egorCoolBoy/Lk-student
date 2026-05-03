using FluentValidation;
using ProfileDocksService.Applicantion.Dtos;

namespace ProfileDocksService.Presentation.Validator;

public class UpdateProfileValidator
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().MinimumLength(2)
                .MaximumLength(100)
                .When(x => x.FirstName != null);

            RuleFor(x => x.LastName)
                .NotEmpty().MinimumLength(2)
                .MaximumLength(100)
                .When(x => x.LastName != null);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,15}$")
                .When(x => x.PhoneNumber != null);

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow)
                .When(x => x.BirthDate != null);

            RuleFor(x => x.Gender)
                .IsInEnum()
                .When(x => x.Gender != null);

            RuleFor(x => x.Citizenship)
                .NotEmpty().MinimumLength(2)
                .When(x => x.Citizenship != null);
        }
    }
}