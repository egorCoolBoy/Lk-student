using DirectoryService.Application.DTO;
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

	public async Task<ImportedDirectroriesStatistic> GetImportedDirectoriesStatistic()
	{
		var lastImport = await _context.DirectoryImportStatistics
			.OrderByDescending(x => x.ImportTime)
			.FirstOrDefaultAsync();
		
		return lastImport;
	}
	
	public async Task<ImportedDirectroriesStatistic> ImportDirectoriesAsync()
	{
		await using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var data = await LoadDirectoriesAsync();

			await ClearDirectoriesAsync();

			await ImportEducationLevelsAsync(data.EducationLevels);

			await ImportEducationDocumentsAsync(
				data.EducationDocuments,
				data.EducationLevels);

			await ImportFacultiesAsync(data.Faculties);

			await ImportProgramsAsync(
				data.Programs,
				data.Faculties,
				data.EducationLevels);

			var statistic = CreateStatistic(data);

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
	}
	
	
	private  class DirectoriesData
	{
		public required List<EducationLevelDto> EducationLevels { get; init; }

		public required List<EducationDocumentDto> EducationDocuments { get; init; }

		public required List<FacultyDto> Faculties { get; init; }

		public required List<ProgramDto> Programs { get; init; }
	}
	private async Task<DirectoriesData> LoadDirectoriesAsync()
	{
		var educationLevelsTask = _kreosoftApi.GetEducationLevels();
		var educationDocumentsTask = _kreosoftApi.GetEducationDocuments();
		var facultiesTask = _kreosoftApi.GetFaculties();
		var programsTask = _kreosoftApi.GetPrograms();

		await Task.WhenAll(
			educationLevelsTask,
			educationDocumentsTask,
			facultiesTask,
			programsTask);

		return new DirectoriesData
		{
			EducationLevels = (await educationLevelsTask).ToList(),
			EducationDocuments = (await educationDocumentsTask).ToList(),
			Faculties = (await facultiesTask).ToList(),
			Programs = (await programsTask).Programs.ToList()
		};
	}
	
	private async Task ClearDirectoriesAsync()
	{
		await _context.EducationDocumentNextLevels.ExecuteDeleteAsync();
		await _context.Programs.ExecuteDeleteAsync();
		await _context.EducationDocuments.ExecuteDeleteAsync();
		await _context.Faculties.ExecuteDeleteAsync();
		await _context.EducationLevels.ExecuteDeleteAsync();
	}
	private async Task ImportEducationLevelsAsync(
		List<EducationLevelDto> educationLevels)
	{
		var entities = educationLevels.Select(x => new EducationLevel
		{
			Id = x.Id,
			Name = x.Name
		});

		await _context.EducationLevels.AddRangeAsync(entities);
	}
	private async Task ImportEducationDocumentsAsync(
		List<EducationDocumentDto> educationDocuments,
		List<EducationLevelDto> educationLevels)
	{
		var levelIds = educationLevels
			.Select(x => x.Id);

		var documentEntities = educationDocuments
			.Where(x => x.EducationLevel != null)
			.Where(x => levelIds.Contains(x.EducationLevel.Id))
			.Select(x => new EducationDocument
			{
				Id = x.Id,
				Name = x.Name,
				CreateTime = EnsureUtc(x.CreateTime),
				EducationLevelId = x.EducationLevel.Id
			});

		await _context.EducationDocuments.AddRangeAsync(documentEntities);

		var links = educationDocuments
			.Where(x => x.NextEducationLevels != null)
			.SelectMany(x => x.NextEducationLevels.Select(level =>
				new EducationDocumentNextLevel
				{
					EducationDocumentId = x.Id,
					EducationLevelId = level.Id
				}));

		await _context.EducationDocumentNextLevels.AddRangeAsync(links);
	}
	
	private async Task ImportFacultiesAsync(
		List<FacultyDto> faculties)
	{
		var entities = faculties.Select(x => new Faculty
		{
			Id = x.Id,
			Name = x.Name,
			CreateTime = EnsureUtc(x.CreateTime)
		});

		await _context.Faculties.AddRangeAsync(entities);
	}
	
	private async Task ImportProgramsAsync(
		List<ProgramDto> programs,
		List<FacultyDto> faculties,
		List<EducationLevelDto> educationLevels)
	{
		var facultyIds = faculties
			.Select(x => x.Id);
			

		var levelIds = educationLevels
			.Select(x => x.Id);

		var entities = programs
			.Where(x => x.Faculty != null)
			.Where(x => x.EducationLevel != null)
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

		await _context.Programs.AddRangeAsync(entities);
	}
	private static ImportedDirectroriesStatistic CreateStatistic(
		DirectoriesData data)
	{
		return new ImportedDirectroriesStatistic
		{
			Id = Guid.NewGuid(),
			ImportTime = DateTime.UtcNow,
			Imported = new ImportedDirectories
			{
				EducationLevels = data.EducationLevels.Count,
				DocumentTypes = data.EducationDocuments.Count,
				Faculties = data.Faculties.Count,
				Programs = data.Programs.Count
			}
		};
	}

	
	
}