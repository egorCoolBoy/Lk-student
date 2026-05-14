using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.Dtos;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion;

public class EducationService : IEducationService
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _fileStorage;

    public EducationService(AppDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<List<GetEducationDto>> GetEducations(Guid userId)
    {
        return await _db.EducationDocuments
            .Where(e => e.UserId == userId)
            .Select(e => new GetEducationDto
            {
                Id = e.Id,
                UserId = e.UserId,
                InstitutionName = e.InstitutionName,
                Level = e.Level,
                Specialty = e.Specialty,
                GraduationDate = e.GraduationDate,
                DiplomaNumber = e.DiplomaNumber,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<GetEducationDto> GetEducation(Guid educationId)
    {
        var education = await _db.EducationDocuments
            .Where(e => e.Id == educationId)
            .Select(e => new GetEducationDto
            {
                Id = e.Id,
                UserId = e.UserId,
                InstitutionName = e.InstitutionName,
                Level = e.Level,
                Specialty = e.Specialty,
                GraduationDate = e.GraduationDate,
                DiplomaNumber = e.DiplomaNumber,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        return education;
    }

    public async Task<GetEducationDto> CreateEducation(Guid userId, CreateEducationDto dto)
    {
        var graduationDate = DateOnly.Parse(dto.GraduationDate);

        var education = new EducationDocument(
            userId,
            dto.InstitutionName,
            dto.Level,
            dto.Specialty,
            graduationDate,
            dto.DiplomaNumber);

        _db.EducationDocuments.Add(education);
        await _db.SaveChangesAsync();

        return new GetEducationDto
        {
            Id = education.Id,
            UserId = education.UserId,
            InstitutionName = education.InstitutionName,
            Level = education.Level,
            Specialty = education.Specialty,
            GraduationDate = education.GraduationDate,
            DiplomaNumber = education.DiplomaNumber,
            CreatedAt = education.CreatedAt,
            UpdatedAt = education.UpdatedAt
        };
    }

    public async Task<GetEducationDto> UpdateEducation(Guid educationId, UpdateEducationDto dto)
    {
        var education = await _db.EducationDocuments.FirstOrDefaultAsync(e => e.Id == educationId);

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        DateOnly? graduationDate = null;
        if (!string.IsNullOrWhiteSpace(dto.GraduationDate))
            graduationDate = DateOnly.Parse(dto.GraduationDate);

        education.UpdateDetails(
            dto.InstitutionName,
            dto.Specialty,
            graduationDate,
            dto.DiplomaNumber,
            dto.Level);
        await _db.SaveChangesAsync();

        return new GetEducationDto
        {
            Id = education.Id,
            UserId = education.UserId,
            InstitutionName = education.InstitutionName,
            Level = education.Level,
            Specialty = education.Specialty,
            GraduationDate = education.GraduationDate,
            DiplomaNumber = education.DiplomaNumber,
            CreatedAt = education.CreatedAt,
            UpdatedAt = education.UpdatedAt
        };
    }

    public async Task DeleteEducation(Guid educationId)
    {
        var education = await _db.EducationDocuments
            .Include(e => e.Scans)
            .FirstOrDefaultAsync(e => e.Id == educationId);

        if (education == null)
            throw new KeyNotFoundException($"Education document with id {educationId} not found");

        foreach (var scan in education.Scans)
        {
            await _fileStorage.DeleteAsync(scan.StorageKey);
        }

        _db.EducationDocuments.Remove(education);
        await _db.SaveChangesAsync();
    }
}
