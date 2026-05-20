using Contracts;
using NotificationsService.Dto;

namespace NotificationsService.EmailService;

public interface IEmailService
{
    Task SendRegistrationEmailAsync(string toEmail);

    public Task SendStatusChangedEmailAsync(string toEmail, string name, string program, string status);

    public Task SendManagerTookApplicantEmailAsync(ManagerTookApplicantNotification notification);

    Task SendDirectoryChangedEmailAsync(List<ProgramChangeEmailDto> programChangeEmailDto);

}