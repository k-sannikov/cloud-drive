using Domain.AccessSystem;

namespace Application.AccessSystem;

public interface IAccessRepository
{
    Task AddAccess(Access access);
    Task<Access> GetAccess(int accessId);
    Task<Access> GetByUserIdAndNodeId(int userId, string nodeId);
    Task<List<Access>> GetByNodeIds(List<string> nodeIds);
    void DeleteAccess(Access access);
}
