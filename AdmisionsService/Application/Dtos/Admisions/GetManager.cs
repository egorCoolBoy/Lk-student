namespace AdmisionsService.Application.Dtos;
using Contracts;
public class GetManager
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string FullName { get; set; }
}