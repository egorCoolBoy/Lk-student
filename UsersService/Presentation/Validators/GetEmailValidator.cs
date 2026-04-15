using FluentValidation;
using UsersService.Presentation.DTO.GetEmails;

namespace UsersService.Presentation.Validators;

public class GetEmailValidator : AbstractValidator<GetEmailsRequest>
{
    public GetEmailValidator()
    {
        RuleFor(x => x.Ids).NotNull();
    }
}