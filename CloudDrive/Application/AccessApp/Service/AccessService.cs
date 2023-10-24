using Application.AccessApp.Repository;
using Application.Exceptions;
using Domain.AccessService;

namespace Application.AccessApp.Service;

public class AccessService : IAccessService
{
    private readonly IAccessRepository _accessRepository;

    public AccessService(IAccessRepository accessRepository)
    {
        _accessRepository = accessRepository;
    }

    public async Task AddAccess(UserForAccess user, NodeForAccess node, bool isOwner = false)
    {
        Access access = new Access();
        access.SetUser(user);
        access.SetNode(node);
        access.SetIsOwner(isOwner);

        await _accessRepository.AddAccess(access);
    }

    public async Task DeleteAccess(UserForAccess user, NodeForAccess node)
    {
        Access? access = await _accessRepository.GetAccess(user, node);

        if (access == null)
        {
            throw new Exception("У пользователя нет доступа к этому узлу");
        }

        _accessRepository.DeleteAccess(access);
    }

    public async Task DeleteAllAccessesByUser(UserForAccess user)
    {
        List<Access> accesses = await _accessRepository.GetAllByUser(user);

        CheckUserAccesses(accesses);

        for (int i = 0; i < accesses.Count; i++)
        {
            _accessRepository.DeleteAccess(accesses[i]);
        }
    }

    public async Task DeleteAllAccessesByNode(NodeForAccess node)
    {
        List<Access> accesses = await _accessRepository.GetAllByNode(node);

        CheckNodeAccesses(accesses);

        for (int i = 0; i < accesses.Count; i++)
        {
            _accessRepository.DeleteAccess(accesses[i]);
        }
    }

    public async Task<bool> UserHasAccess(UserForAccess user, List<NodeForAccess> nodes)
    {
        List<Access> accesses = await _accessRepository.GetAccesses(user, nodes);

        return accesses.Any();
    }
    public async Task<List<Access>> GetAllUsersAccesses(UserForAccess user)
    {
        List<Access> accesses = await _accessRepository.GetAllByUser(user);

        return accesses;
    }

    public async Task<List<Access>> GetAllAccessesToNode(NodeForAccess node)
    {
        List<Access> accesses = await _accessRepository.GetAllByNode(node);

        return accesses;
    }

    public async Task<Access> GetAccess(UserForAccess user, NodeForAccess node)
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
