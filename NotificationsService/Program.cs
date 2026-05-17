using MassTransit;
using NotificationsService;
using NotificationsService.EmailService;
using NotificationsService.SmtpSettings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ManagerConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("manager-created-queue", e =>
        {
            e.ConfigureConsumer<ManagerConsumer>(context);
        });
    });
});


var host = builder.Build();
host.Run();