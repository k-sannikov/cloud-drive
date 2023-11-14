using Application.ProxyServices;
using CloudDrive.Auth;
using CloudDrive.Dto;
using FluentValidation;
using Infrastructure;
using Infrastructure.AccessSystem;
using Infrastructure.Auth;
using Infrastructure.FileSystem;
using Infrastructure.Folders;
using Infrastructure.ProxyServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CloudDrive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = NewMethod(args);

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

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                OpenApiSecurityScheme jwtSecurityScheme = new()
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });


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

            services.AddHttpClient();

            AuthSettings authSettings = configuration.GetSection("Auth").Get<AuthSettings>();
            services.AddScoped(sp => authSettings);

            ProxySettings proxySettings = configuration.GetSection("Proxy").Get<ProxySettings>();
            services.AddScoped(sp => proxySettings);

            services.Configure<Neo4jSettings>(configuration.GetSection("Neo4j"));

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<ApiKeyAuthFilter>();

            services.AddFileSystemRepositories();
            services.AddFileSystemServices();
            services.AddFoldersServices();

            services.AddValidatorsFromAssemblyContaining<CreateFolderDto>();
            services.AddValidatorsFromAssemblyContaining<CreateLinkDto>();
            services.AddValidatorsFromAssemblyContaining<EditLinkDto>();
            services.AddValidatorsFromAssemblyContaining<EditFolderDto>();
            services.AddValidatorsFromAssemblyContaining<RegisterDto>();
            services.AddValidatorsFromAssemblyContaining<LoginDto>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokenDto>();

            services.AddDatabaseFoundations(configuration);

            services.AddAccessSystemRepositories();
            services.AddAccessSystemServices();

            services.AddAuthRepositories();
            services.AddAuthServices();

            services.AddProxyServices();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authSettings.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = TokenService.GetSymmetricSecurityKey(authSettings.Key),
                        ValidateIssuerSigningKey = true,

                        RequireExpirationTime = true,
                    };
                });

            WebApplication app = builder.Build();


            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(builder =>
            {
                //builder.WithOrigins("http://localhost:3000");
                //builder.AllowCredentials();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static WebApplicationBuilder NewMethod(string[] args)
        {
            return WebApplication.CreateBuilder(args);
        }
    }
}