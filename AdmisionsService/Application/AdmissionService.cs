using AdmisionsService.Application.Dtos;
using AdmisionsService.Application.Dtos.Admisions;
using AdmisionsService.Application.Interfaces;
using AdmisionsService.Infrastructure;
using AdmisionsService.Infrastructure.AppDbContext;
using Microsoft.Extensions.Options;

namespace AdmisionsService.Application;

public class AdmissionService : IAdmissionService
{
    private readonly AppDbContext _db;
    private readonly IUsersServiceApi _usersService;
    private readonly IDocumentAPI _documentService;
    private readonly IDirectoriesAPI _directoriesService;
    private readonly IConfiguration _configuration; 
    private readonly NOptions _options;

    public AdmissionService(AppDbContext db, IUsersServiceApi usersService, IDocumentAPI documentService,
        IDirectoriesAPI directoriesService, IConfiguration configuration, IOptions<NOptions> options)
    {
        _db = db;
        _usersService = usersService;
        _documentService = documentService;
        _directoriesService = directoriesService;
        _configuration = configuration;
        _options = options.Value;
    }

    public async Task<AdmissionCreatedResponse> CreateAdmisison(CreateAdmisisonDto dto)
    {
        var n = _options.N;
        var admisionCount =  _db.Admissions.Count(a => a.ApplicantId == dto.UserId);
        if (admisionCount == n)
            throw new InvalidOperationException($"max count of admisions is  {n}");
        
        
        var programTask = _directoriesService.GetProgramByIdAsync(dto.ProgramId);
        var profileTask = _documentService.GetProfileAsync(dto.UserId);
        var educationTask = _documentService.GetDocxAsync(dto.UserId);
        var docksTask = _documentService.GetDocxAsync(dto.UserId);

        await Task.WhenAll(programTask, profileTask, docksTask);

        var program = await programTask;
        var profile = await profileTask;
        var docks = await docksTask;
        var education = await educationTask;

        //несколько документов у абитуриента может быть
        
        
        throw new NotImplementedException();
    }



    private  bool CheckEducationLevel(EducationDocumentTypesDto educatDocx, ProgramDto program)
    {
        var programEducationLevel = program.EducationLevel.Id;

        return educatDocx.EducationLevel.Id == programEducationLevel
               || educatDocx.NextEducationLevels.Any(x => x.Id == programEducationLevel);
    }
}