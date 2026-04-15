using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Services;

public class FacultiesService : IFacultiesService
{
    private readonly AppDbContext _context;

    public FacultiesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FacultyDto>> GetFacultiesAsync()
    {
        var faculties = await _context.Faculties.Select(x=>new FacultyDto
            {
                Id = x.Id,
                Name = x.Name,
                CreateTime = x.CreateTime,
            }
            ).ToListAsync();
        return faculties;
    }

    public async Task<FacultyDto> GetFacultyByIdAsync(Guid id)
    {
        var faculty = await _context.Faculties.Select(f=>new FacultyDto
        {
            Id = f.Id,
            Name = f.Name,
            CreateTime = f.CreateTime,
        }).FirstOrDefaultAsync(f => f.Id == id);
        if (faculty == null)
            throw new InvalidOperationException("Faculty not found");
        
        return faculty;
        
    }
}