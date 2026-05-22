using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Dtos.Admisions;
using AdmisionsService.Application.Interfaces;
using AdmisionsService.Application.ManagerFacultyService;
using AdmisionsService.Domain;
using AdmisionsService.Infrastructure;
using AdmisionsService.Infrastructure.AppDbContext;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdmisionsService.Application;

public class AdmissionService : IAdmissionService
{
    private readonly AppDbContext _db;
    private readonly IUsersServiceApi _usersService;
    private readonly IDocumentAPI _documentService;
    private readonly IDirectoriesAPI _directoriesService;
    private readonly NOptions _options;
    private readonly IPublishEndpoint _publish;

    public AdmissionService(AppDbContext db, IUsersServiceApi usersService, IDocumentAPI documentService,
        IDirectoriesAPI directoriesService, IOptions<NOptions> options,IPublishEndpoint publish)
    {
        _db = db;
        _usersService = usersService;
        _documentService = documentService;
        _directoriesService = directoriesService;
        _options = options.Value;
        _publish = publish;
    }

    public async Task<GetAdmissionDto> CreateAdmissison(CreateAdmisisonDto dto)
    {
        var n = _options.N;
        var admisionCount = _db.Admissions.Count(a => a.ApplicantId == dto.UserId);
        if (admisionCount == n)
            throw new InvalidOperationException($"max count of admisions is  {n}");
        
        if (dto.Priority > n || dto.Priority < 1)
            throw new InvalidOperationException($"minimum priority is {n} and maximum priority is 1");



        var programTask = _directoriesService.GetProgramByIdAsync(dto.ProgramId);
        var profileTask = _documentService.GetProfileAsync(dto.UserId);
        var educationTask = _documentService.GetDocxAsync(dto.UserId);
        var documentTypeTask = _directoriesService.GetEducationDocumentTypeAsync();

        await Task.WhenAll(programTask, profileTask,educationTask,documentTypeTask);

        var program = await programTask;
        var profile = await profileTask;
        var educations = await educationTask;
        var documentTypes = await documentTypeTask;

        //проверка что заявки поданы на программу с одинаковым уровнем обрзования
        var currentProgramEducationLevelId = program.EducationLevel.Id;

        var hasDifferentEducationLevel = await _db.Admissions
            .Where(a => a.ApplicantId == dto.UserId)
            .AnyAsync(a => a.EducationLevelId != currentProgramEducationLevelId);

        if (hasDifferentEducationLevel)
        {
            throw new InvalidOperationException("Applications can only be submitted to programs of the same education level");
        }
        
        // проверка уровня образования абитуриента
        if (!CheckEducationLevel(educations,program,documentTypes))
        {
            throw new InvalidOperationException("Applicant education level does not meet program requirements");
        }
        
        //Создание заявки
        var admission = new Admission()
        {
            ProgramId = program.Id,
            ProgramName = program.Name,
            EducationLevelId = program.EducationLevel.Id,
            EducationLevel = program.EducationLevel.Name,
            FacultyId = program.Faculty.Id,
            FacultyName = program.Faculty.Name,
            ApplicantId = dto.UserId,
            ApplicantFullName = profile.LastName + " " + profile.FirstName + " " + profile.Patronymic,
            Priority = dto.Priority,
        };
        
        await _db.Admissions.AddAsync(admission);
        await _db.SaveChangesAsync();
        
        return AdmisisonMapper(admission);
        
    }

    public async Task DeleteAdmissison(DeleteAdmission dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.UserdId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        _db.Admissions.Remove(admission);
        await _db.SaveChangesAsync();
    }

    public async Task<GetAdmissionDto> TakeAdmission(ManagerAdmission dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.ApplicantId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        
        if (admission.ManagerId.HasValue)
            throw new InvalidOperationException("admission already has manager");
        
        var checkFaculty = await _db.ManagerFaculties.FirstOrDefaultAsync(mf=>mf.ManagerId == dto.ManagerId);
        if(checkFaculty == null)
            throw new KeyNotFoundException("Faculty not found");
        if (checkFaculty.FacultyId != admission.FacultyId)
            throw new KeyNotFoundException("different faculties");
        
        var manager = await _usersService.GetManagerAsync(dto.ManagerId);
        admission.ManagerId = manager.Id;
        admission.ManagerFullName = manager.FullName;
        var message = new ManagerTookApplicant()
        {
            ApplicantId = admission.ApplicantId,
            ManagerId = admission.ManagerId.Value,
            ApplicantName = admission.ApplicantFullName,
            ManagerName = admission.ManagerFullName,
            ProgramName = admission.ProgramName,
        };
        await _publish.Publish(message);
        await _db.SaveChangesAsync();
       
        return AdmisisonMapper(admission);
    }

    public async Task<GetAdmissionDto> UnTakeAdmission(ManagerAdmission dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.ApplicantId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        
        var manager = await _usersService.GetManagerAsync(dto.ManagerId);
        admission.ManagerId = null;
        admission.ManagerFullName = null;
        await _db.SaveChangesAsync();
       
        return AdmisisonMapper(admission);
    }

    public async Task<List<GetAdmissionDto>> GetAdmissions(AdmissionsQueryParams p)
    {
        var query = _db.Admissions.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var search = p.Search.Trim().ToLower();

            query = query.Where(x =>
                x.ApplicantFullName.ToLower().Contains(search));
        }
        
        if (p.ProgramId.HasValue)
        {
            query = query.Where(x => x.ProgramId == p.ProgramId);
        }
        
        if (p.FacultyIds != null && p.FacultyIds.Count > 0)
        {
            query = query.Where(x => p.FacultyIds.Contains(x.FacultyId));
        }
        
        if (p.Status.HasValue)
        {
            query = query.Where(x => x.Status == p.Status);
        }
        
        if (p.OnlyWithoutManager)
        {
            query = query.Where(x => x.ManagerId == null);
        }
        else
        {
            query = query.Where(x => x.ManagerId != null);
        }

        if (p.ManagerId.HasValue)
        {
            query = query.Where(x => x.ManagerId == p.ManagerId);
        }
        
        query = p.SortByUpdatedAt?.ToLower() switch
        {
            "asc"  => query.OrderBy(x => x.UpdatedAt),
            "desc" => query.OrderByDescending(x => x.UpdatedAt),
            _      => query.OrderByDescending(x => x.UpdatedAt)
        };
        
        var skip = (p.Page - 1) * p.PageSize;

        query = query
            .Skip(skip)
            .Take(p.PageSize);

        return AdmisisonMapper(await query.ToListAsync());
    }

    public async Task<GetAdmissionDto> GetAdmissionByIds(GetAdmissionByIds dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.ApplicantId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        return AdmisisonMapper(admission);
    }

    public async Task<GetAdmissionDto> UpdatePriority(UpdatePriorityDto dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.ApplicantId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        
        if (admission.Status == AdmisionStatus.Closed)
            throw new InvalidOperationException("Admission is closed");
        
        var n = _options.N;
        if (dto.Priority > n || dto.Priority < 1)
            throw new InvalidOperationException($"minimum priority is {n} and maximum priority is 1");
        admission.Priority = dto.Priority;
        await _db.SaveChangesAsync();
        return AdmisisonMapper(admission);
    }

    public async Task<GetAdmissionDto> UpdateStatus(UpdateStatusDto dto)
    {
        var admission = await _db.Admissions.Where(x => x.ProgramId == dto.ProgramId && x.ApplicantId == dto.ApplicantId)
            .FirstOrDefaultAsync();
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");
        admission.Status = dto.Status;
        var message = new AdmissionStatusChanged()
        {
            ApplicantId = admission.ApplicantId,
            ApplicantName = admission.ApplicantFullName,
            Status = admission.Status.ToString(),
            ProgramName = admission.ProgramName
        };
        await _publish.Publish(message);
        await _db.SaveChangesAsync();
        return AdmisisonMapper(admission);
    }

    public async Task<List<GetAdmissionDto>> GetAdmissionByApplicantId(Guid id)
    {
        var admissions =  await _db.Admissions.Where(x => x.ApplicantId == id).ToListAsync();
        return AdmisisonMapper(admissions);
    }
    
    
    static public bool CheckEducationLevel(List<EducationDocxDto>educations, ProgramDto program,List<EducationDocumentTypesDto> documentTypes)
    {
        //айди документов абитуриента
        var educationIds = educations.Select(e => e.EducationTypeId).ToList();
        //айди уровень обр программы
        var programsEducationsIds = program.EducationLevel.Id;
        //айди уровней образования доступные для абитуриентов с его документами
        var documentIds = documentTypes
            .Where(x => educationIds.Contains(x.Id))
            .SelectMany(x =>
                new List<int> { x.EducationLevel.Id }
                    .Concat(x.NextEducationLevels.Select(n => n.Id)))
            .Distinct()
            .ToList();
        return documentIds.Contains(programsEducationsIds);
    }

    private GetAdmissionDto AdmisisonMapper(Admission admission)
    {
        return new GetAdmissionDto
        {
            ApplicantId = admission.ApplicantId,
            ApplicantFullName = admission.ApplicantFullName,

            ProgramId = admission.ProgramId,
            ProgramName = admission.ProgramName,

            FacultyId = admission.FacultyId,
            FacultyName = admission.FacultyName,

            ManagerId = admission.ManagerId,
            ManagerFullName = admission.ManagerFullName,

            EducationLevel = admission.EducationLevel,
            EducationLevelId = admission.EducationLevelId,

            Priority = admission.Priority,

            Status = admission.Status,

            CreatedAt = admission.CreatedAt,
            UpdatedAt = admission.UpdatedAt
        };
    }
    private List<GetAdmissionDto> AdmisisonMapper(List<Admission> admissions)
    {

        return  admissions.Select(x => new GetAdmissionDto
        {
            ApplicantId = x.ApplicantId,
            ApplicantFullName = x.ApplicantFullName,

            ProgramId = x.ProgramId,
            ProgramName = x.ProgramName,

            FacultyId = x.FacultyId,
            FacultyName = x.FacultyName,

            ManagerId = x.ManagerId,
            ManagerFullName = x.ManagerFullName,

            EducationLevel = x.EducationLevel,
            EducationLevelId = x.EducationLevelId,

            Priority = x.Priority,

            Status = x.Status,

            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt
        }).ToList();
    }
}