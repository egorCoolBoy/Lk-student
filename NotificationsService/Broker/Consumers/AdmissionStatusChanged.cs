using Contracts;
using MassTransit;
using NotificationsService.EmailService;

namespace NotificationsService;

public class AdmissionStatusChangedNotificationConsumer : IConsumer<AdmissionStatusChangedNotification>
{
    private readonly IEmailService _emailService;

    public AdmissionStatusChangedNotificationConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<AdmissionStatusChangedNotification> context)
    {
        var message = context.Message;
        await _emailService.SendStatusChangedEmailAsync(message.ApplicantEmail, message.ApplicantName, message.ProgramName, message.Status);
        Console.WriteLine($"{message.ApplicantName} {message.ProgramName} {message.Status}");
    }
}