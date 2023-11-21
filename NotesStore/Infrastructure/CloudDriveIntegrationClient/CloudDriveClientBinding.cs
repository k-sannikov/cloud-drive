using Application.CloudDriveIntegrationClient;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CloudDriveIntegrationClient;

public static class CloudDriveClientBinding
{
    public static void AddCloudDriveClient(this IServiceCollection services)
    {
        services.AddHttpClient<ICloudDriveClient, CloudDriveClient>();
    }
}
