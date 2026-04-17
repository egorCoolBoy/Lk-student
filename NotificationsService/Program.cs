using NotificationsService;
using NotificationsService.EmailService;
using NotificationsService.SmtpSettings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();