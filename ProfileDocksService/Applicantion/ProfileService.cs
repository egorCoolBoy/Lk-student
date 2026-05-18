using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.Dtos;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Domain.Enums;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _db;

    public ProfileService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<GetProfileDto> GetProfile(Guid userId)
    {
        var profile = await _db.Profiles
            .Where(x => x.UserId == userId)
            .Select(profile => new GetProfileDto
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Patronymic = profile.Patronymic,
                PhoneNumber = profile.PhoneNumber,
                BirthDate = profile.BirthDate,
                Gender = profile.Gender,
                Citizenship = profile.Citizenship,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
            })
            .FirstOrDefaultAsync();

        if (profile == null)
            throw new KeyNotFoundException($"Profile with userId {userId} not found");

        return profile;
    }

    public async Task<UpdateProfileDto> UpdateProfile(UpdateProfileDto newProfile)
    {
        var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.UserId == newProfile.UserId);
        
        if (profile == null)
            throw new KeyNotFoundException($"Profile with userId {newProfile.UserId} not found");
      
        profile.UpdateProfileInfo(newProfile.FirstName,
            newProfile.LastName,
            newProfile.Patronymic,
            newProfile.BirthDate,
            newProfile.Gender,
            newProfile.Citizenship,
            newProfile.PhoneNumber);
        await _db.SaveChangesAsync();
        
        return new UpdateProfileDto()
        {
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Patronymic = profile.Patronymic,
            PhoneNumber = profile.PhoneNumber,
            BirthDate = profile.BirthDate,
            Gender = profile.Gender,
            Citizenship = profile.Citizenship,
            UpdatedAt = profile.UpdatedAt,
        };
    }
    
    public async Task CreateProfile(Guid userId)
    {
        await _db.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO ""Profiles"" (""UserId"", ""CreatedAt"")
        VALUES ({userId}, now())
        ON CONFLICT (""UserId"") DO NOTHING;
    ");
    }
}