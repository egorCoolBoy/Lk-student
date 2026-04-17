using NotificationsService.Dto;

namespace NotificationsService.EmailService;

public interface IEmailService
{
    Task SendRegistrationEmailAsync(string toEmail);

    Task SendStatusChangedEmailAsync(string toEmail, string status, string program);

    Task SendEmailAsync(string toEmail, string subject, string body);

    Task SendDirectoryChangedEmailAsync(List<ProgramChangeEmailDto> programChangeEmailDto);

}