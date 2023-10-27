using Application.AccessApp.Repository;
using Application.AccessApp.Service;
using Infrastructure.AccessService.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.AccessService;

public static class ConfigureService
{
    public static void AddAccessService(this IServiceCollection services)
    {
        services.AddScoped<IAccessService, Application.AccessApp.Service.AccessService>();
    }

    public static void AddAccessRepository(this IServiceCollection services)
    {
        services.AddScoped<IAccessRepository, AccessRepository>();
    }
}
