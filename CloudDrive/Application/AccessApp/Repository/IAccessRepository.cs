using Domain.AccessService;
using Domain.FileSystem;
using Domain.UserService;

namespace Application.AccessApp.Repository;

public interface IAccessRepository
{
    public Task AddAccess(Access access);
    public void DeleteAccess(Access access);
    public Task<List<Access>> GetAccesses(User user, List<Node> nodes);
    public Task<List<Access>> GetAllByUser(User user);
    public Task<List<Access>> GetAllByNode(Node node);
    public Task<Access> GetAccess(User user, Node node);
}
