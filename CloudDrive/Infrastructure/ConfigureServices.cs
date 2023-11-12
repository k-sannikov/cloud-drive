using Application.Foundations;
using Infrastructure.Database;
using Infrastructure.Foundations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddDatabaseFoundations(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("PostgreSQL");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();


        Neo4jSettings neo4jSettings = services.BuildServiceProvider().GetRequiredService<IOptions<Neo4jSettings>>().Value;

        IDriver driver = GraphDatabase.Driver(neo4jSettings.ServiceUrl, AuthTokens.Basic(neo4jSettings.Username, neo4jSettings.Password));

        services.AddSingleton(driver);
    }
}
