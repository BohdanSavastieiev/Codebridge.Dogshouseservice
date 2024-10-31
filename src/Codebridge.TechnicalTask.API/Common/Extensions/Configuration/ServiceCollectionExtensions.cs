using System.Text.Json;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Exceptions;
using Codebridge.TechnicalTask.API.Common.Middlewares;
using Codebridge.TechnicalTask.API.Common.Settings;
using FluentValidation;

namespace Codebridge.TechnicalTask.API.Common.Extensions.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddConfiguration(configuration)
            .AddCoreServices()
            .AddRateLimiting(configuration);

        return services;
    }

    private static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddCorsPolicy()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
            .AddAutoMapper(typeof(DependencyInjection).Assembly)
            .AddEndpointDefinitions(typeof(DependencyInjection).Assembly);
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceSettings>(
            configuration.GetSection("ServiceInfo")
            ?? throw new ApiConfigurationException(nameof(ServiceSettings)));

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.AllowAllCorsPolicy, policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }
}