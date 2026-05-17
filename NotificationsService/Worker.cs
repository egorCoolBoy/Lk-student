using NotificationsService.EmailService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEmailService _emailService;

    public Worker(ILogger<Worker> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }
    }
}