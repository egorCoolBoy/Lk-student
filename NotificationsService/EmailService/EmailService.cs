using Contracts;
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

    public async Task SendManagerTookApplicantEmailAsync(ManagerTookApplicantNotification notification)
    {
        var subject = "Приемная кампания";
        var body = $"Здравствуйте, {notification.ApplicantName}! За ваше поступление на программу: {notification.ProgramName}, отвечает менеджер:{notification.ManagerName} ";
        await SendEmailAsync(notification.ApplicantEmail, subject, body);
        
        var subjectManeger = "Приемная кампания";
        var bodyManeger = $"Здравствуйте, {notification.ManagerName}! Вы отвечаете ха поступление на программу: {notification.ProgramName}, данного абитуриента: {notification.ApplicantName} ";
        await SendEmailAsync(notification.ManagerEmail, subjectManeger, bodyManeger);
    }

    public async Task SendStatusChangedEmailAsync(string toEmail,string name,string program,string status)
    {
        var subject = "Изменение статуса поступления";
        var body = $"Здравствуйте, {name}, ваш новый статус поступления по программе: {program}  теперь:{status}";

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

        message.To.Add("ega02022007@gmail.com");

        await client.SendMailAsync(message);
    }
}