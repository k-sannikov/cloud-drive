using Application.AccessSystem;
using Domain.AccessSystem;
using Domain.Auth;
using Domain.FileSystem;

namespace Application.FileSystem;

public class FileSystemService : IFileSystemService
{
    private readonly IFileSystemRepository _fileSystemRepository;
    private readonly IAccessService _accessService;

    public FileSystemService(IFileSystemRepository fileSystemRepository, IAccessService accessService)
    {
        _fileSystemRepository = fileSystemRepository;
        _accessService = accessService;
    }

    public async Task<T> GetNode<T>(string nodeId) where T : Node, new()
    {
        IReadOnlyList<T> nodes = await _fileSystemRepository
            .GetNodes<T>(new List<string>() { nodeId });

        return nodes.FirstOrDefault();
    }

    public async Task CreateNode(string parentId, Node newNode)
    {
        Node node = await GetNode<Node>(parentId);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
        }
        await _fileSystemRepository.AddNodeWithRelation(newNode, parentId);
    }

    public async Task RenameNode(string id, string name)
    {
        await _fileSystemRepository.RenameNode(id, name);
    }

    public async Task DeleteNode(string nodeId)
    {
        Node node = await GetNode<Node>(nodeId);
        if (node is null)
        {
            throw new Exception($"Node with id: {nodeId} not exist");
        }

        List<Access> access = await _accessService.GetByNodeId(nodeId);

        bool isRoot = access.FirstOrDefault()?.IsRoot ?? false;

        if (isRoot)
        {
            throw new Exception($"Cannot delete the root directory");
        }

        await _fileSystemRepository.DeleteNodeWithChildren(nodeId);
    }

    public async Task EditNode<T>(Node modifiedNode) where T : Node, new()
    {
        Node node = await GetNode<Node>(modifiedNode.Id);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
        }

        modifiedNode.Type = node.Type;

        await _fileSystemRepository.EditNode<T>(modifiedNode);
    }

    public async Task<IReadOnlyList<Node>> GetChildNodes(string nodeId)
    {
        Node node = await GetNode<Node>(nodeId);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
        }

        return await _fileSystemRepository.GetChildsNodes(nodeId);
    }

    public async Task<IReadOnlyList<Node>> GetParentsNodes(string nodeId)
    {
        Node node = await GetNode<Node>(nodeId);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
        }

        return await _fileSystemRepository.GetParentsNodes(nodeId);
    }

    public async Task<IReadOnlyList<Node>> GetAvailableNodes(string userId)
    {
        List<Access> accesses = await _accessService.GetAvailableNodes(userId);

        IReadOnlyList<string> nodeIds = accesses.Select(a => a.NodeId).ToList();

        IReadOnlyList<Node> nodes = await _fileSystemRepository.GetNodes<Node>(nodeIds);

        return nodes;
    }
}
