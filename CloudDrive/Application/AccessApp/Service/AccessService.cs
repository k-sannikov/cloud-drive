using Application.AccessApp.Repository;
using Application.Exceptions;
using Domain.AccessService;
using Domain.FileSystem;
using Domain.UserService;

namespace Application.AccessApp.Service;

public class AccessService : IAccessService
{
    private readonly IAccessRepository _accessRepository;

    public AccessService(IAccessRepository accessRepository)
    {
        _accessRepository = accessRepository;
    }

    public async Task AddAccess(User user, Node node, bool isOwner = false)
    {
        Access access = new Access();
        access.SetUser(user);
        access.SetNode(node);
        access.SetIsNotOwner();

        if (isOwner)
        {
            access.SetIsOwner();
        }

        await _accessRepository.AddAccess(access);
    }

    public async Task DeleteAccess(User user, Node node)
    {
        Access? access = await _accessRepository.GetAccess(user, node);

        if (access == null)
        {
            throw new Exception("У пользователя нет доступа к этому узлу");
        }

        _accessRepository.DeleteAccess(access);
    }

    public async Task DeleteAllAccessesByUser(User user)
    {
        List<Access> accesses = await _accessRepository.GetAllByUser(user);

        CheckUserAccesses(accesses);

        for (int i = 0; i < accesses.Count; i++)
        {
            _accessRepository.DeleteAccess(accesses[i]);
        }
    }

    public async Task DeleteAllAccessesByNode(Node node)
    {
        List<Access> accesses = await _accessRepository.GetAllByNode(node);

        CheckNodeAccesses(accesses);

        for (int i = 0; i < accesses.Count; i++)
        {
            _accessRepository.DeleteAccess(accesses[i]);
        }
    }

    public async Task<bool> UserHasAccess(User user, List<Node> nodes)
    {
        List<Access> accesses = await _accessRepository.GetAccesses(user, nodes);

        if (accesses.Count == 0)
        {
            return false;
        }

        return true;
    }
    public async Task<List<Access>> GetAllUsersAccesses(User user)
    {
        List<Access> accesses = await _accessRepository.GetAllByUser(user);

        CheckUserAccesses(accesses);

        return accesses;
    }

    public async Task<List<Access>> GetAllAccessesToNode(Node node)
    {
        List<Access> accesses = await _accessRepository.GetAllByNode(node);

        CheckNodeAccesses(accesses);

        return accesses;
    }

    public async Task<Access> GetAccess(User user, Node node)
    {
        Access access = await _accessRepository.GetAccess(user, node);

        return access;
    }

    private void CheckUserAccesses(List<Access> accesses)
    {
        if (accesses.Count == 0)
        {
            throw new UserDoesntHaveAccessException();
        }
    }

    private void CheckNodeAccesses(List<Access> accesses)
    {
        if (accesses.Count == 0)
        {
            throw new NoAccessesToTheNodeException();
        }
    }
}
