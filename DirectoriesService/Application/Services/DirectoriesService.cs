using DirectoryService.Application.Interface;
using DirectoryService.Domain.Entities;
using DirectoryService.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using ProgramEntity = DirectoryService.Domain.Entities.Program;

namespace DirectoryService.Application.Services;

public class DirectoriesService : IDirectoriesService
{
	private static readonly SemaphoreSlim ImportLock = new(1, 1);
	private readonly IKreosoftApi _kreosoftApi;
	private readonly AppDbContext _context;

	private static DateTime EnsureUtc(DateTime value)
	{
		return value.Kind switch
		{
			DateTimeKind.Utc => value,
			DateTimeKind.Local => value.ToUniversalTime(),
			_ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
		};
	}

	public DirectoriesService(IKreosoftApi kreosoftApi, AppDbContext context)
	{
		_kreosoftApi = kreosoftApi;
		_context = context;
	}

	public async Task<ImportedDirectroriesStatistic> ImportDirectoriesAsync()
	{
		await ImportLock.WaitAsync();
		await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var educationLevelsTask = _kreosoftApi.GetEducationLevels();
            var educationDocumentsTask = _kreosoftApi.GetEducationDocuments();
            var facultiesTask = _kreosoftApi.GetFaculties();
            var programsTask = _kreosoftApi.GetPrograms();

            await Task.WhenAll(educationLevelsTask, educationDocumentsTask, facultiesTask, programsTask);
            
            var educationLevels = (await educationLevelsTask).ToList();
            var educationDocuments = (await educationDocumentsTask).ToList();
            var faculties = (await facultiesTask).ToList();
            var programs = (await programsTask).Programs.ToList();

            _context.ChangeTracker.Clear();
            //удаление всех записей
            _context.EducationDocumentNextLevels.RemoveRange(_context.EducationDocumentNextLevels);
            _context.Programs.RemoveRange(_context.Programs);
            _context.EducationDocuments.RemoveRange(_context.EducationDocuments);
            _context.Faculties.RemoveRange(_context.Faculties);
            _context.EducationLevels.RemoveRange(_context.EducationLevels);

            await _context.SaveChangesAsync();

            var levelIds = educationLevels.Select(x => x.Id).ToHashSet();

            var levelEntities = educationLevels.Select(x => new EducationLevel
            {
                Id = x.Id,
                Name = x.Name
            });

            _context.EducationLevels.AddRange(levelEntities);

            var documentEntities = educationDocuments
                .Where(x => x.EducationLevel != null && levelIds.Contains(x.EducationLevel.Id))
                .Select(x => new EducationDocument
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreateTime = EnsureUtc(x.CreateTime),
                    EducationLevelId = x.EducationLevel.Id
                });

            _context.EducationDocuments.AddRange(documentEntities);

            var documentLinks = educationDocuments
                .Where(d => d.NextEducationLevels != null)
                .SelectMany(d => d.NextEducationLevels.Select(n => new EducationDocumentNextLevel
                {
                    EducationDocumentId = d.Id,
                    EducationLevelId = n.Id
                }));

            _context.EducationDocumentNextLevels.AddRange(documentLinks);

            var facultyIds = faculties.Select(x => x.Id).ToHashSet();

            var facultyEntities = faculties.Select(x => new Faculty
            {
                Id = x.Id,
                Name = x.Name,
                CreateTime = EnsureUtc(x.CreateTime)
            });

            _context.Faculties.AddRange(facultyEntities);

            var programEntities = programs
                .Where(x => x.Faculty != null && x.EducationLevel != null)
                .Where(x => facultyIds.Contains(x.Faculty.Id))
                .Where(x => levelIds.Contains(x.EducationLevel.Id))
                .Select(x => new ProgramEntity
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Language = x.Language,
                    EducationForm = x.EducationForm,
                    CreateTime = EnsureUtc(x.CreateTime),
                    FacultyId = x.Faculty.Id,
                    EducationLevelId = x.EducationLevel.Id
                });

            _context.Programs.AddRange(programEntities);

            await _context.SaveChangesAsync();

            var statistic = new ImportedDirectroriesStatistic
            {
                Id = Guid.NewGuid(),
                ImportTime = DateTime.UtcNow,
                Imported = new ImportedDirectories
                {
                    EducationLevels = educationLevels.Count,
                    DocumentTypes = educationDocuments.Count,
                    Faculties = faculties.Count,
                    Programs = programs.Count
                }
            };

            _context.DirectoryImportStatistics.Add(statistic);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return statistic;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            ImportLock.Release();
        }
	}
}