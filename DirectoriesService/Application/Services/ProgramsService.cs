using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Infrastructure.AppDbContext;
using DirectoryService.Presentation.Dto;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Services;

public class ProgramsService : IProgramsService
{
    private readonly AppDbContext _context;

    public ProgramsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProgramsResponseDto> GetProgramsAsync(ProgramsQueryDto queryDto)
{
    if (queryDto.Page < 1)
        throw new ArgumentException("Page must be greater than 0");

    if (queryDto.Size < 1)
        throw new ArgumentException("Size must be greater than 0");

    var query = _context.Programs
        .Include(x => x.Faculty)
        .Include(x => x.EducationLevel)
        .AsQueryable();
    
    if (queryDto.FacultyId.HasValue)
    {
        query = query.Where(x => x.FacultyId == queryDto.FacultyId);
    }
    
    if (queryDto.EducationLevelId.HasValue)
    {
        query = query.Where(x => x.EducationLevelId == queryDto.EducationLevelId);
    }
    
    if (!string.IsNullOrWhiteSpace(queryDto.EducationForm))
    {
        query = query.Where(x =>
            x.EducationForm == queryDto.EducationForm);
    }
    
    if (!string.IsNullOrWhiteSpace(queryDto.Language))
    {
        query = query.Where(x =>
            x.Language == queryDto.Language);
    }
    
    if (!string.IsNullOrWhiteSpace(queryDto.Search))
    {
        var search = queryDto.Search.Trim().ToLower();

        query = query.Where(x =>
            x.Name.ToLower().Contains(search) ||
            x.Code.ToLower().Contains(search));
    }

    query = query
        .OrderBy(x => x.Name)
        .ThenBy(x => x.Id);

    var totalCount = await query.CountAsync();

    var programs = await query
        .Skip((queryDto.Page - 1) * queryDto.Size)
        .Take(queryDto.Size)
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
            Size = queryDto.Size,
            Count = totalCount,
            Current = queryDto.Page
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