﻿using Domain.FileSystem;

namespace Application.FileSystem;

public class FileSystemService : IFileSystemService
{
    private readonly IFileSystemRepository _fileSystemRepository;

    public FileSystemService(IFileSystemRepository fileSystemRepository)
    {
        _fileSystemRepository = fileSystemRepository;
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

    public async Task RenameNode(Node modifiedNode)
    {
        await _fileSystemRepository.RenameNode(modifiedNode.Id, modifiedNode.Name);
    }

    public async Task DeleteNode(string nodeId)
    {
        Node node = await GetNode<Node>(nodeId);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
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
}
