using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Infrastructure.Database;

namespace Infrastructure.Migrations;

class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfiguration config = GetConfig();
        string connectionString = config.GetConnectionString("PostgreSQL");
        var optionalBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionalBuilder.UseNpgsql(connectionString, assembly => assembly.MigrationsAssembly("Infrastructure.Migrations"));

        return new AppDbContext(optionalBuilder.Options);
    }

    private static IConfiguration GetConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true);

        return builder.Build();
    }
}
