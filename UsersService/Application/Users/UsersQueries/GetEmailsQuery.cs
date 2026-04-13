namespace UsersService.Application.Users.Queries;

public class GetEmailsQuery
{
    public List<Guid> Ids { get; set; } = new();
}