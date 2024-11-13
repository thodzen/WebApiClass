using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Software.Api.Configuration;
public static class ServicesExtensions
{
    public static WebApplicationBuilder AddCustomFeatureManagement(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiFeatureManagementOptions>(
            builder.Configuration.GetSection(ApiFeatureManagementOptions.FeatureManagement));
        builder.Services.AddFeatureManagement();
        return builder;
    }
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(DefaultJsonOptions.Configure);
        services.AddAuthentication().AddJwtBearer(); // look in the config for your environment and get the settings.
        services.AddFeatureManagement();
        services.AddSingleton(() => TimeProvider.System); // .net 8 and later, a first class abstraction for the clock. 
        services.AddHttpContextAccessor(); // always on thing for. Scoped services, make the http context available.


        return services;
    }

    public static IServiceCollection AddCustomOasGeneration(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header with bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer ",
                            In = ParameterLocation.Header
                        },
                        []
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            });
        services.AddFluentValidationRulesToSwagger();
        return services;
    }
}