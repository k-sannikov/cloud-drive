using Application.FileSystem;
using Domain.FileSystem;

namespace Application.Folders;

public class FoldersService : IFoldersService
{
    private readonly IFileSystemRepository _fileSystemRepository;
    private readonly IFileSystemService _fileSystemService;

    public FoldersService(IFileSystemRepository fileSystemRepository, IFileSystemService fileSystemService)
    {
        _fileSystemRepository = fileSystemRepository;
        _fileSystemService = fileSystemService;
    }

    public async Task CreateFolder(Folder folder, string parentId)
    {
        Node node = await _fileSystemService.GetNode<Node>(parentId);
        if (node is null)
        {
            throw new Exception($"Node with id: {node.Id} not exist");
        }
        await _fileSystemRepository.AddNodeWithRelation(folder, parentId);
    }
}