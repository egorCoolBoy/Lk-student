using ProfileDocksService.Domain.Enums;

namespace ProfileDocksService.Applicantion.Dtos;

public class GetProfileDto
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? Citizenship { get; set; }
    public DateTime CreatedAt { get;  set; }
    public DateTime? UpdatedAt { get; set; }
}