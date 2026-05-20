using Contracts;
using MassTransit;
using UsersService.Application.Users.Queries;

namespace UsersService.Infrastructure.Broker;

public class ManagerTookApplicantConsumer : IConsumer<ManagerTookApplicant>
{
    private readonly IUsersService _usersService;

    public ManagerTookApplicantConsumer(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task Consume(ConsumeContext<ManagerTookApplicant> context)
    {
        var message = context.Message;
        var listIds = new List<Guid>(){message.ApplicantId,message.ManagerId};
        var idsQuery = new GetEmailsQuery()
        {
            Ids = listIds
        };
        
        var result = await  _usersService.GetEmails(idsQuery);
        if (!result.Emails.TryGetValue(message.ApplicantId, out var applicantEmail))
            throw new KeyNotFoundException("Applicant email not found");
        
        var messageTo = new ManagerTookApplicantNotification()    
        {
            ApplicantEmail = result.Emails[message.ApplicantId],
            ManagerEmail = result.Emails[message.ManagerId],
            ManagerName = message.ManagerName,
            ApplicantName = message.ApplicantName,
            ProgramName = message.ProgramName
        };
        await context.Publish(messageTo);
    }
}