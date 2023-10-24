using Domain.AccessService;

namespace Application.AccessApp.Repository;

public interface IAccessRepository
{
    public Task AddAccess(Access access);
    public void DeleteAccess(Access access);
    public Task<List<Access>> GetAccesses(UserForAccess user, List<NodeForAccess> nodes);
    public Task<List<Access>> GetAllByUser(UserForAccess user);
    public Task<List<Access>> GetAllByNode(NodeForAccess node);
    public Task<Access> GetAccess(UserForAccess user, NodeForAccess node);
}
