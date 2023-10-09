using CloudDrive.Dto;
using FluentValidation;
using Infrastructure;
using Infrastructure.FileSystem;
using Infrastructure.Folders;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CloudDrive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Lufi",
                        Description = "",
                    }
                );

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });


            services.AddFileSystemRepositories();
            services.AddFileSystemServices();
            services.AddFoldersServices();

            services.AddValidatorsFromAssemblyContaining<CreateFolderDto>();
            services.AddValidatorsFromAssemblyContaining<CreateLinkDto>();
            services.AddValidatorsFromAssemblyContaining<EditLinkDto>();
            services.AddValidatorsFromAssemblyContaining<EditNameNodeDto>();

            services.Configure<Neo4jSettings>(configuration.GetSection("Neo4j"));

            services.AddDatabaseFoundations(configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}