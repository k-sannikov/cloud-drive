using Domain.AccessService;

namespace Application.AccessApp.Service;

public interface IAccessService
{
    public Task AddAccess(UserForAccess user, NodeForAccess node, bool isOwner = false);
    public Task DeleteAccess(UserForAccess user, NodeForAccess node);
    public Task DeleteAllAccessesByUser(UserForAccess user);
    public Task DeleteAllAccessesByNode(NodeForAccess node);
    public Task<bool> UserHasAccess(UserForAccess user, List<NodeForAccess> nodes);
    public Task<List<Access>> GetAllUsersAccesses(UserForAccess user);
    public Task<List<Access>> GetAllAccessesToNode(NodeForAccess node);
    public Task<Access> GetAccess(UserForAccess user, NodeForAccess node);
}
