namespace UsersService.Presentation.DTO.GetEmails;

public class GetEmailsRequest
{
    public List<Guid> Ids { get; set; } = new();
}