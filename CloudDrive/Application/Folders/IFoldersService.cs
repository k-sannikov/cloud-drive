using Domain.FileSystem;

namespace Application.Folders;

public interface IFoldersService
{
    Task CreateFolder(Folder folder, string parentId);
}