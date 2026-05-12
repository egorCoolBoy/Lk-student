using Microsoft.EntityFrameworkCore;
using ProfileDocksService.Applicantion.Dtos;
using ProfileDocksService.Domain.Entities;
using ProfileDocksService.Infrastructure.AppDbContext;

namespace ProfileDocksService.Applicantion;

public class PassportService : IPassportService
{
    private readonly AppDbContext _db;

    public PassportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<GetPassportDto>> GetPassports(Guid userId)
    {
        return await _db.PassportDocuments
            .Where(p => p.UserId == userId)
            .Select(p => new GetPassportDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Series = p.Series,
                Number = p.Number,
                IssuedBy = p.IssuedBy,
                IssuedDate = p.IssuedDate,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<GetPassportDto> GetPassport(Guid passportId)
    {
        var passport = await _db.PassportDocuments
            .Where(p => p.Id == passportId)
            .Select(p => new GetPassportDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Series = p.Series,
                Number = p.Number,
                IssuedBy = p.IssuedBy,
                IssuedDate = p.IssuedDate,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        return passport;
    }

    public async Task<GetPassportDto> CreatePassport(Guid userId, CreatePassportDto dto)
    {
        var issuedDate = DateOnly.Parse(dto.IssuedDate);

        var passport = new PassportDocument(
            userId,
            dto.Series,
            dto.Number,
            dto.IssuedBy,
            issuedDate);

        _db.PassportDocuments.Add(passport);
        await _db.SaveChangesAsync();

        return new GetPassportDto
        {
            Id = passport.Id,
            UserId = passport.UserId,
            Series = passport.Series,
            Number = passport.Number,
            IssuedBy = passport.IssuedBy,
            IssuedDate = passport.IssuedDate,
            CreatedAt = passport.CreatedAt,
            UpdatedAt = passport.UpdatedAt
        };
    }

    public async Task<GetPassportDto> UpdatePassport(Guid passportId, UpdatePassportDto dto)
    {
        var passport = await _db.PassportDocuments.FirstOrDefaultAsync(p => p.Id == passportId);

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        DateOnly? issuedDate = null;
        if (!string.IsNullOrWhiteSpace(dto.IssuedDate))
            issuedDate = DateOnly.Parse(dto.IssuedDate);

        passport.UpdateDetails(
            dto.Series,
            dto.Number,
            dto.IssuedBy,
            issuedDate);

        _db.PassportDocuments.Update(passport);
        await _db.SaveChangesAsync();

        return new GetPassportDto
        {
            Id = passport.Id,
            UserId = passport.UserId,
            Series = passport.Series,
            Number = passport.Number,
            IssuedBy = passport.IssuedBy,
            IssuedDate = passport.IssuedDate,
            CreatedAt = passport.CreatedAt,
            UpdatedAt = passport.UpdatedAt
        };
    }

    public async Task DeletePassport(Guid passportId)
    {
        var passport = await _db.PassportDocuments.FirstOrDefaultAsync(p => p.Id == passportId);

        if (passport == null)
            throw new KeyNotFoundException($"Passport with id {passportId} not found");

        _db.PassportDocuments.Remove(passport);
        await _db.SaveChangesAsync();
    }
}
