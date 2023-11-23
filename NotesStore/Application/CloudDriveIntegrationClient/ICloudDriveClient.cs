using Infrastructure.CloudDriveIntegrationClient.RequestDto;

namespace Application.CloudDriveIntegrationClient;

public interface ICloudDriveClient
{
    Task<bool> CreateNode(CreateNodeDto createNodeDto);
    Task<bool> RenameNode(string id, string name);
    Task<bool> DeleteNode(string id);
}
