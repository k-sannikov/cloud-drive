using Application.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FileSystem;

public static class FoldersBinding
{
    public static void AddFileSystemRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFileSystemRepository, FileSystemRepository>();
    }

    public static void AddFileSystemServices(this IServiceCollection services)
    {
        services.AddScoped<IFileSystemService, FileSystemService>();
    }
}
