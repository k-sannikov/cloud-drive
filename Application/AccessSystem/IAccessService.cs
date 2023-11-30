using Domain.AccessSystem;

namespace Application.AccessSystem;

public interface IAccessService
{
    Task AddAccess(Access access);
    Task DeleteAccess(string userId, string nodeId);
    Task DeleteAccess(int accessId);
    Task<bool> HasAccess(string userId, string nodeId);
    Task<Access> GetAccess(string userId, string nodeId);
    Task<Access> GetAccess(int accessId);
    Task<Access> GetRootAccess(string userId);
    Task<List<Access>> GetByNodeId(string nodeId);
    Task<List<Access>> GetAvailableNodes(string userId);
}
