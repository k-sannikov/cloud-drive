using Domain.FileSystem;

namespace Application.FileSystem;

public interface IFileSystemService
{
    Task<T> GetNode<T>(string nodeId) where T : Node, new();
    Task<IReadOnlyList<Node>> GetChildNodes(string nodeId);
    Task<IReadOnlyList<Node>> GetParentsNodes(string nodeId);
    Task CreateNode(string parentId, Node newNode);
    Task DeleteNode(string nodeId);
    Task EditNode<T>(Node modifiedNode) where T : Node, new();
}
