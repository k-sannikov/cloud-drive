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
            throw new Exception("User not exist");
        }

        await _accessRepository.AddAccess(access);
    }

    public async Task DeleteAccess(int userId, string nodeId)
    {
        Access access = await _accessRepository.GetByUserIdAndNodeId(userId, nodeId);

        if (access is null)
        {
            throw new Exception("Access not exist");
        }

        if (access.IsOwner)
        {
            throw new Exception("Сannot delete the owner access");
        }

        _accessRepository.DeleteAccess(access);
    }

    public async Task<bool> HasAccess(int userId, string nodeId)
    {
        IReadOnlyList<Node> nodes = await _fileSystemRepository.GetParentsNodes(nodeId);

        List<string> nodeIds = nodes.Select(n => n.Id).ToList();

        nodeIds.Add(nodeId);

        List<Access> accesses = await _accessRepository.GetByNodeIds(nodeIds);

        Access access = accesses.Where(a => a.UserId == userId).FirstOrDefault();

        return access is not null;
    }
}
