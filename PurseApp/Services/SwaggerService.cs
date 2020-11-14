using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace PurseApp.Services
{
    public static class SwaggerService
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo {Title = "Purse App V1", Version = "v1"});
                s.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                
                s.AddSecurityRequirement(new OpenApiSecurityRequirement 
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
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}
