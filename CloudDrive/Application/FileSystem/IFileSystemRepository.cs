using Domain.FileSystem;

namespace Application.FileSystem;

public interface IFileSystemRepository
{
    Task AddNode(Node node);
    Task<IReadOnlyList<T>> GetNodes<T>(IEnumerable<string> nodeIds) where T : Node, new();
    Task RenameNode(string nodeId, string newName);
    Task EditNode<T>(Node node) where T : Node, new();
    Task AddNodeWithRelation(Node node, string parentId);
    Task DeleteNodeWithChildren(string nodeId);
    Task<IReadOnlyList<Node>> GetParentsNodes(string nodeId);
    Task<IReadOnlyList<Node>> GetChildsNodes(string nodeId);
}
