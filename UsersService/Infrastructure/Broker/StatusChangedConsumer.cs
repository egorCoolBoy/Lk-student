using Contracts;
using MassTransit;
using UsersService.Application.Users.Queries;

namespace UsersService.Infrastructure.Broker;

public class AdmissionStatusChangedConsumer : IConsumer<AdmissionStatusChanged>
{ 
    private readonly IUsersService _usersService;

    public AdmissionStatusChangedConsumer(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task Consume(ConsumeContext<AdmissionStatusChanged> context)
    {
        var message = context.Message;
        var listIds = new List<Guid>(){message.ApplicantId};
        var idsQuery = new GetEmailsQuery()
        {
            Ids = listIds
        };
    
        var result = await  _usersService.GetEmails(idsQuery);
        if (!result.Emails.TryGetValue(message.ApplicantId, out var applicantEmail))
            throw new KeyNotFoundException("Applicant email not found");
    
        var messageTo = new AdmissionStatusChangedNotification()    
        {
            ApplicantEmail = result.Emails[message.ApplicantId],
            ApplicantName = message.ApplicantName,
            ProgramName = message.ProgramName,
            Status = message.Status
        };
        
        await context.Publish(messageTo);
    }
}