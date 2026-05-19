using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Services;

public class EducationDocumentService : IEducationDocumentService
{
	private readonly AppDbContext _context;

	public EducationDocumentService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<EducationDocumentDto>> GetEducationDocumentsAsync()
	{
		var documents = await _context.EducationDocuments
			.Include(x => x.NextLevels)
			.ThenInclude(x => x.EducationLevel)
			.Join(
				_context.EducationLevels,
				document => document.EducationLevelId,
				level => level.Id,
				(document, level) => new EducationDocumentDto
				{
					Id = document.Id,
					CreateTime = document.CreateTime,
					Name = document.Name,
					EducationLevel = new EducationLevelDto
					{
						Id = level.Id,
						Name = level.Name
					},
					NextEducationLevels = document.NextLevels
						.Select(link => new EducationLevelDto
						{
							Id = link.EducationLevel.Id,
							Name = link.EducationLevel.Name
						})
						.ToList()
				})
			.ToListAsync();

		return documents;
	}

	public async Task<EducationDocumentDto> GetEducationDocumentByIdAsync(Guid id)
	{
		var document = await _context.EducationDocuments
			.Include(x => x.NextLevels)
			.ThenInclude(x => x.EducationLevel).Where(x=>x.Id == id)
			.Join(
				_context.EducationLevels,
				item => item.EducationLevelId,
				level => level.Id,
				(item, level) => new EducationDocumentDto
				{
					Id = item.Id,
					CreateTime = item.CreateTime,
					Name = item.Name,
					EducationLevel = new EducationLevelDto
					{
						Id = level.Id,
						Name = level.Name
					},
					NextEducationLevels = item.NextLevels
						.Select(link => new EducationLevelDto
						{
							Id = link.EducationLevel.Id,
							Name = link.EducationLevel.Name
						})
						.ToList()
				})
			.FirstOrDefaultAsync();

		if (document == null)
			throw new InvalidOperationException("Education document not found");

		return document;
	}
}