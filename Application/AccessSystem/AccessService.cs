using Application.Auth;
using Application.FileSystem;
using Domain.AccessSystem;
using Domain.Auth;
using Domain.FileSystem;

namespace Application.AccessSystem;

public class AccessService : IAccessService
{
    private readonly IAccessRepository _accessRepository;
    private readonly IFileSystemRepository _fileSystemRepository;
    private readonly IUserRepository _userRepository;

    public AccessService(IAccessRepository accessRepository,
        IFileSystemRepository fileSystemRepository,
        IUserRepository userRepository)
    {
        _accessRepository = accessRepository;
        _fileSystemRepository = fileSystemRepository;
        _userRepository = userRepository;
    }

    public async Task AddAccess(Access access)
    {
        User user = await _userRepository.GetUser(access.UserId);

        if (user is null)
        {
            throw new Exception("Пользователь не определен");
        }

        await _accessRepository.AddAccess(access);
    }

    public async Task DeleteAccess(string userId, string nodeId)
    {
        Access access = await _accessRepository.GetByUserIdAndNodeId(userId, nodeId);

        if (access is null)
        {
            throw new Exception("Доступ не был обнаружен");
        }

        if (access.IsOwner)
        {
            throw new Exception("Невозможно удалить доступ");
        }

        _accessRepository.DeleteAccess(access);
    }

    public async Task DeleteAccess(int accessId)
    {
        Access access = await _accessRepository.GetAccess(accessId);

        if (access is null)
        {
            throw new Exception("Доступ не был обнаружен");
        }

        if (access.IsOwner)
        {
            throw new Exception("Невозможно удалить доступ");
        }

        _accessRepository.DeleteAccess(access);
    }

    public async Task<bool> HasAccess(string userId, string nodeId)
    {
        Access access = await GetAccess(userId, nodeId);

        return access is not null;
    }

    public async Task<Access> GetAccess(string userId, string nodeId)
    {
        IReadOnlyList<Node> nodes = await _fileSystemRepository.GetParentsNodes(nodeId);

        List<string> nodeIds = nodes.Select(n => n.Id).ToList();

        nodeIds.Add(nodeId);

        List<Access> accesses = await _accessRepository.GetByNodeIds(nodeIds);

        Access access = accesses.Where(a => a.UserId == userId).FirstOrDefault();

        return access;
    }

    public async Task<Access> GetAccess(int accessId)
    {
        return await _accessRepository.GetAccess(accessId);
    }

    public async Task<Access> GetRootAccess(string userId)
    {
        return await _accessRepository.GetRootByUserId(userId);
    }

    public async Task<List<Access>> GetByNodeId(string nodeId)
    {
        return await _accessRepository.GetByNodeIds(new List<string>() { nodeId });
    }

    public async Task<List<Access>> GetAvailableNodes(string userId)
    {
        return await _accessRepository.GetAvailableNodes(userId);
    }
}
