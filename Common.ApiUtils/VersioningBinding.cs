using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Common.ApiUtils
{
    public static class VersioningBinding
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services
                .AddApiVersioning(config =>
                {
                    config.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
                })
                .AddVersionedApiExplorer(option =>
                {
                    option.GroupNameFormat = "'v'VVV";
                    option.SubstituteApiVersionInUrl = true;
                });

            return services;
        }
    }
}