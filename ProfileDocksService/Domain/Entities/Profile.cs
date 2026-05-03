using ProfileDocksService.Domain.Enums;

namespace ProfileDocksService.Domain.Entities;

public class Profile
{
    public Guid UserId { get; private set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? Citizenship { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }

    public Profile(Guid userId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdateProfileInfo(
        string? firstName,
        string? lastName,
        string? patronymic,
        DateTime? birthDate,
        Gender? gender,
        string? citizenship,
        string? phoneNumber)
    {
        if (firstName != null && firstName.Trim().Length < 2)
            throw new ArgumentException("FirstName too short");

        if (lastName != null && lastName.Trim().Length < 2)
            throw new ArgumentException("LastName too short");

        if (birthDate.HasValue && birthDate > DateTime.UtcNow)
            throw new ArgumentException("BirthDate cannot be in the future");
        
        FirstName = string.IsNullOrWhiteSpace(firstName) ? null : firstName.Trim();
        LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName.Trim();
        Patronymic = string.IsNullOrWhiteSpace(patronymic) ? null : patronymic.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();

        BirthDate = birthDate;
        Gender = gender;
        Citizenship = citizenship;

        UpdatedAt = DateTime.UtcNow;
    }
}