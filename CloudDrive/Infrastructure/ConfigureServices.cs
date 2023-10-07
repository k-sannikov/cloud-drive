using Infrastructure.Foundations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Foundations;
using Infrastructure.Database;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddDatabaseFoundations(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("LocalPostgreSQL");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
