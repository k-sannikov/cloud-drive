using Application.Folders;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Folders;

public static class FoldersBinding
{
    public static void AddFoldersServices(this IServiceCollection services)
    {
        services.AddScoped<IFoldersService, FoldersService>();
    }
}
