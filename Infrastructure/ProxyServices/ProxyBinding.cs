using Application.ProxyServices;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ProxyServices;

public static class ProxyBinding
{
    public static void AddProxyServices(this IServiceCollection services)
    {
        services.AddScoped<IProxyService, ProxyService>();
    }
}
