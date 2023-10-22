using Domain.AccessService;
using Domain.FileSystem;
using Domain.UserService;

namespace Application.AccessApp.Service;

public interface IAccessService
{
    public Task AddAccess(User user, Node node, bool isOwner = false);
    public Task DeleteAccess(User user, Node node);
    public Task DeleteAllAccessesByUser(User user);
    public Task DeleteAllAccessesByNode(Node node);
    public Task<bool> UserHasAccess(User user, List<Node> nodes);
    public Task<List<Access>> GetAllUsersAccesses(User user);
    public Task<List<Access>> GetAllAccessesToNode(Node node);
    public Task<Access> GetAccess(User user, Node node);
}
