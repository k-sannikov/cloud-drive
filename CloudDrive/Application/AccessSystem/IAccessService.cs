using Domain.AccessSystem;

namespace Application.AccessSystem;

public interface IAccessService
{
    Task AddAccess(Access access);
    Task DeleteAccess(string userId, string nodeId);
    Task<bool> HasAccess(string userId, string nodeId);
    Task<Access> GetRootAccess(string userId);
}
