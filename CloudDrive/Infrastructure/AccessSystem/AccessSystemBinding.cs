using Application.AccessSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.AccessSystem;

public static class AccessSystemBinding
{
    public static void AddAccessSystemRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessRepository, AccessRepository>();
    }

    public static void AddAccessSystemServices(this IServiceCollection services)
    {
        services.AddScoped<IAccessService, AccessService>();
    }
}
