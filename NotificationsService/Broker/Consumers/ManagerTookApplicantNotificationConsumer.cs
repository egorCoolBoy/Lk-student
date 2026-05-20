using Contracts;
using MassTransit;
using NotificationsService.EmailService;

namespace NotificationsService;

public class ManagerTookApplicantNotificationConsumer : IConsumer<ManagerTookApplicantNotification>
{
    private readonly IEmailService _emailService;

    public ManagerTookApplicantNotificationConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<ManagerTookApplicantNotification> context)
    {
        var message = context.Message;
        await _emailService.SendManagerTookApplicantEmailAsync(message);
    }
}
