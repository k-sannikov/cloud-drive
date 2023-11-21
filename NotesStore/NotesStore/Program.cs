using FluentValidation;
using Infrastructure;
using Infrastructure.Notes;
using Microsoft.OpenApi.Models;
using NotesStore.Authentication;
using NotesStore.Dto;
using Infrastructure.CloudDriveIntegrationClient;

namespace NotesStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "The Api Key to access the Api",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header,
                };

                var requirement = new OpenApiSecurityRequirement
                {
                    { scheme, new List<string>() }
                };

                c.AddSecurityRequirement(requirement);
            });

            CloudDriveApiSettings cloudDriveApiSettings = builder.Configuration.GetSection("CloudDrive").Get<CloudDriveApiSettings>();
            builder.Services.AddScoped(sp => cloudDriveApiSettings);

            builder.Services.AddScoped<ApiKeyAuthFilter>();

            builder.Services.AddDatabaseFoundations(builder.Configuration);
            builder.Services.AddNoteRepositories();
            builder.Services.AddNoteService();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateNoteDto>();

            builder.Services.AddCloudDriveClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = "";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}