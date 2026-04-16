using DirectoryService.Application.DTO;
using DirectoryService.Application.Interface;
using DirectoryService.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Services;

public class EducationLevelService : IEducationLevelService
{
    private readonly AppDbContext _context;

    public EducationLevelService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EducationLevelDto>> GetEducationLevelsAsync()
    {
        var levels = await _context.EducationLevels
            .Select(x => new EducationLevelDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        return levels;
    }

    public async Task<EducationLevelDto> GetEducationLevelByIdAsync(int id)
    {
        var level = await _context.EducationLevels
            .Select(x => new EducationLevelDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (level == null)
            throw new InvalidOperationException("Education level not found");

        return level;
    }
}