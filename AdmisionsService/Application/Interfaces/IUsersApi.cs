using AdmisionsService.Application.Dtos;

namespace AdmisionsService.Application.Interfaces;

public interface IUsersServiceApi
{
    public Task<GetManager> GetManagerAsync(Guid id);
}