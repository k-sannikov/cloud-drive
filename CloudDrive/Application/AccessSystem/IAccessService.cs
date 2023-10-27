using Domain.AccessSystem;

namespace Application.AccessSystem;

public interface IAccessService
{
    Task AddAccess(Access access);
    Task DeleteAccess(int userId, string nodeId);
    Task<bool> HasAccess(int userId, string nodeId);
}
