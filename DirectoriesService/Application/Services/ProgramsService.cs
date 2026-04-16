using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Services;

public class ProgramsService : IProgramsService
{
    private readonly AppDbContext _context;

    public ProgramsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProgramsResponseDto> GetProgramsAsync(int page, int size)
    {
        if (page < 1)
            throw new ArgumentException("Page must be greater than 0");

        if (size < 1)
            throw new ArgumentException("Size must be greater than 0");

        var query = _context.Programs
            .Include(x => x.Faculty)
            .Include(x => x.EducationLevel)
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var programs = await query
            .Skip((page - 1) * size)
            .Take(size)
            .Select(x => new ProgramDto
            {
                Id = x.Id,
                CreateTime = x.CreateTime,
                Name = x.Name,
                Code = x.Code,
                Language = x.Language,
                EducationForm = x.EducationForm,
                Faculty = new FacultyDto
                {
                    Id = x.Faculty.Id,
                    Name = x.Faculty.Name,
                    CreateTime = x.Faculty.CreateTime
                },
                EducationLevel = new EducationLevelDto
                {
                    Id = x.EducationLevel.Id,
                    Name = x.EducationLevel.Name
                }
            })
            .ToListAsync();

        return new ProgramsResponseDto
        {
            Programs = programs,
            Pagination = new Pagination
            {
                Size = size,
                Count = totalCount,
                Current = page
            }
        };
    }

    public async Task<ProgramDto> GetProgramByIdAsync(Guid id)
    {
        var program = await _context.Programs
            .Include(x => x.Faculty)
            .Include(x => x.EducationLevel)
            .Select(x => new ProgramDto
            {
                Id = x.Id,
                CreateTime = x.CreateTime,
                Name = x.Name,
                Code = x.Code,
                Language = x.Language,
                EducationForm = x.EducationForm,
                Faculty = new FacultyDto
                {
                    Id = x.Faculty.Id,
                    Name = x.Faculty.Name,
                    CreateTime = x.Faculty.CreateTime
                },
                EducationLevel = new EducationLevelDto
                {
                    Id = x.EducationLevel.Id,
                    Name = x.EducationLevel.Name
                }
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (program == null)
            throw new InvalidOperationException("Program not found");

        return program;
    }
}