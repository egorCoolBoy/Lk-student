using AdmisionsService.Application.Dtos.Admisions;

namespace AdmisionsService.Application.Interfaces;

public interface IAdmissionService
{
    public Task<GetAdmissionDto> CreateAdmissison(CreateAdmisisonDto dto);
    public Task DeleteAdmissison(DeleteAdmission dto);
    public Task<GetAdmissionDto> TakeAdmission(ManagerAdmission dto);
    public Task<GetAdmissionDto> UnTakeAdmission(ManagerAdmission dto);
    public Task<List<GetAdmissionDto>> GetAdmissions(AdmissionsQueryParams query);
    public Task<GetAdmissionDto> GetAdmissionByIds(GetAdmissionByIds dto);
    public Task<GetAdmissionDto> UpdatePriority(UpdatePriorityDto dto);
    public Task<GetAdmissionDto> UpdateStatus(UpdateStatusDto dto);
}