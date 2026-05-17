using Contracts;
using MassTransit;
using NotificationsService.EmailService;

namespace NotificationsService;

public class ManagerConsumer : IConsumer<ManagerCreated>
{
    private readonly IEmailService _emailService;

    public ManagerConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<ManagerCreated> context)
    {
        await _emailService.SendRegistrationEmailAsync("ega02022007@gmail.com");
        Console.WriteLine($"Registration email sent to {context.Message.Email}");
    }
} 
