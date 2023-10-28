using Application.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Auth;

public static class AuthBinding
{
    public static void AddAuthRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}
