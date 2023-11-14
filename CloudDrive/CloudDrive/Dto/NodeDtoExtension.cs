using Domain.FileSystem;

namespace CloudDrive.Dto;

public static class NodeDtoExtension
{
    public static NodeDto ToDto(this Node node)
    {
        return new NodeDto()
        {
            Id = node.Id,
            Name = node.Name,
            Type = node.Type.ToString(),
        };
    }

    public static Folder ToDomain(this CreateFolderDto body)
    {
        return new Folder()
        {
            Id = Guid.NewGuid().ToString(),
            Name = body.Name,
            Type = NodeType.Folder.ToString(),
        };
    }

    public static Folder ToDomain(this EditFolderDto body, string id)
    {
        return new Folder()
        {
            Id = id,
            Name = body.Name,
        };
    }

    public static Link ToDomain(this CreateLinkDto body)
    {
        return new Link()
        {
            Id = Guid.NewGuid().ToString(),
            Name = body.Name,
            Type = NodeType.Link.ToString(),
            Description = body.Description,
            Url = body.Url,
        };
    }

    public static Link ToDomain(this EditLinkDto body, string id)
    {
        return new Link()
        {
            Id = id,
            Name = body.Name,
            Type = NodeType.Link.ToString(),
            Description = body.Description,
            Url = body.Url,
        };
    }

    public static Node ToDomain(this CreateNodeDto body)
    {
        return new Node()
        {
            Id = body.Id,
            Name = body.Name,
            Type = body.Type,
        };
    }

    public static NodeListDto ToDto(this IReadOnlyList<Node> nodes, Node parent)
    {
        return new NodeListDto()
        {
            ParentId = parent.Id,
            Name = parent.Name,
            Nodes = nodes.Select(n => n.ToDto()).ToList(),
        };
    }
}
