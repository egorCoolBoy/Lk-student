using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.Dtos;
using ProfileDocksService.Applicantion.FileStorage;
using ProfileDocksService.Applicantion.Interface;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion;

public class EducationService : IEducationService
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _fileStorage;
    private readonly IDirectoriesAPI _directoriesAPI;

    public EducationService(AppDbContext db, IFileStorage fileStorage, IDirectoriesAPI directoriesAPI)
    {
        _db = db;
        _fileStorage = fileStorage;
        _directoriesAPI = directoriesAPI;
    }

    public async Task<List<GetEducationDto>> GetEducations(Guid userId)
    {
        return await _db.EducationDocuments
            .Where(e => e.UserId == userId)
            .Select(e => new GetEducationDto
            {
                Id = e.Id,
                UserId = e.UserId,
                EducationTypeId = e.EducationTypeId,
                EducationTypeName = e.EducationName,
                Level = e.LevelName,
                Specialty = e.Specialty,
                LevelId = e.LevelId,
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
                EducationTypeId = e.EducationTypeId,
                EducationTypeName = e.EducationName,
                Level = e.LevelName,
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
        var educationType = await _directoriesAPI.GetEducationDocumentTypeByIdAsync(dto.EducationTypeId);

        var education = new EducationDocument(
            userId,
            educationType.EducationLevel.Name,
            dto.Specialty,
            graduationDate,
            dto.DiplomaNumber,educationType.EducationLevel.Id,educationType.Id,educationType.Name);

        _db.EducationDocuments.Add(education);
        await _db.SaveChangesAsync();

        return new GetEducationDto
        {
            Id = education.Id,
            UserId = education.UserId,
            Level = education.LevelName,
            LevelId = education.LevelId,
            EducationTypeId = education.EducationTypeId,
            EducationTypeName = education.EducationName,
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
        
        var educationType = await _directoriesAPI.GetEducationDocumentTypeByIdAsync(dto.EducationTypeId);
        education.UpdateDetails(
            dto.Specialty,
            graduationDate,
            dto.DiplomaNumber,
            educationType.EducationLevel.Name, educationType.EducationLevel.Id,educationType.Id,educationType.Name);
        await _db.SaveChangesAsync();

        return new GetEducationDto
        {
            Id = education.Id,
            UserId = education.UserId,
            Level = education.LevelName,
            LevelId = education.LevelId,
            EducationTypeId = education.EducationTypeId,
            EducationTypeName = education.EducationName,
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
