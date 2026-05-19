using AdmisionsService.Application.Dtos.Admisions;

namespace AdmisionsService.Application.Interfaces;

public interface IAdmissionService
{
    public Task<AdmissionCreatedResponse> CreateAdmisison(CreateAdmisisonDto dto);
}