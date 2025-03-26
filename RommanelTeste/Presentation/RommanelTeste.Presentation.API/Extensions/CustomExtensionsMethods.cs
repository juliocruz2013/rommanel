using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Persistence;
using RommanelTeste.Presentation.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace RommanelTeste.Presentation.API.Extensions;

public static class CustomExtensionsMethods
{
    public static IServiceCollection AddCustomFramework(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            opt.Filters.Add(typeof(ValidateModelStateAttribute));
        })
        .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        )
        .AddJsonOptions(option =>
            option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        )
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddSingleton(option =>
        {
            return Configuration._configuration;
        });

        services.AddHeaderPropagation(options =>
        {
            options.Headers.Add("X-TraceId");
        });

        return services;
    }

    public static IServiceCollection AddCustomOpenAPI(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<RommanelTesteContext>()
                .AddDefaultTokenProviders();

        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "RommanelTeste", Version = "v1" });

            options.DocumentFilter<LowercaseDocumentFilter>();
            options.IncludeXmlComments(xmlPath);
            options.CustomSchemaIds(type => type.ToString());
            options.EnableAnnotations();
            options.ResolveConflictingActions(d => d.First());

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "Token",
                In = ParameterLocation.Header,
                Description = @"Cabeçalho de autorização usando esquema Bearer.
                      Digite 'Bearer' e insira seu token no campo abaixo.
                      Exemplo: 'Bearer 12345abcdef'"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthenticationAPI(this IServiceCollection services)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JWTSecret))
            };
        });

        return services;
    }

    public class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var (key, value) in swaggerDoc.Paths)
            {
                paths.Add(key.ToLower(), value);
            }
            swaggerDoc.Paths = paths;
        }
    }
}
