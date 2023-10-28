using Domain.AccessSystem;

namespace Application.AccessSystem;

public interface IAccessRepository
{
    Task AddAccess(Access access);
    Task<Access> GetAccess(int accessId);
    Task<Access> GetByUserIdAndNodeId(string userId, string nodeId);
    Task<Access> GetRootByUserId(string userId);
    Task<List<Access>> GetByNodeIds(List<string> nodeIds);
    void DeleteAccess(Access access);
}
