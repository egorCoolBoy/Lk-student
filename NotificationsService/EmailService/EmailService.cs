using NotificationsService.Dto;

namespace NotificationsService.EmailService;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly SmtpSettings.SmtpSettings _settings;

    public EmailService(IOptions<SmtpSettings.SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendRegistrationEmailAsync(string toEmail)
    {
        var subject = "Регистрация";
        var body = $"Здравствуйте, {toEmail}! Вас успешно зарегистрировали. ";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendStatusChangedEmailAsync(string toEmail, string faculty,string program)
    {
        var subject = "Изменение статуса поступления";
        var body = $"Здравствуйте, {toEmail}, Проверьте новый статус поступления по программе: {program} факультета: {faculty}";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendDirectoryChangedEmailAsync(List<ProgramChangeEmailDto> programChangeEmailDto)
    {
        var subject = "Изменение приемной кампании";
        foreach (var item in programChangeEmailDto)
        {
            string body = $"Здравствуйте, {item.Email}, программу которую вы выбрали больше нет: {item.Program}";
            await SendEmailAsync(item.Email, subject, body);
        }
        
    }
    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(_settings.From),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        message.To.Add(toEmail);

        await client.SendMailAsync(message);
    }
}