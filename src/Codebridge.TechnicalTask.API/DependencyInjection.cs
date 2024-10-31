using System.Reflection;
using System.Text.Json;
using System.Threading.RateLimiting;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Exceptions;
using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.API.Common.Interfaces;
using Codebridge.TechnicalTask.API.Common.Middlewares;
using Codebridge.TechnicalTask.API.Common.Settings;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;

namespace Codebridge.TechnicalTask.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddEndpointDefinitions(typeof(DependencyInjection).Assembly);
        services.AddEndpointsApiExplorer();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        services.Configure<ServiceSettings>(
            configuration.GetSection("ServiceInfo")
            ?? throw new ApiConfigurationException(nameof(ServiceSettings)));
        
        var rateLimitSettings = configuration
                                    .GetSection(nameof(RateLimitSettings))
                                    .Get<RateLimitSettings>()
                                ?? throw new ApiConfigurationException(nameof(RateLimitSettings));

        services.Configure<RateLimitSettings>(
            configuration.GetSection(nameof(RateLimitSettings)));

        services.AddRateLimiting(rateLimitSettings);
        
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });
        
        return services;
    }
    
    private static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        RateLimitSettings rateLimitSettings)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            
            options.AddFixedWindowLimiter(ApiConstants.GlobalRateLimitGroup, config =>
            {
                if (rateLimitSettings.PermitLimit <= 0)
                    throw new ApiConfigurationException(
                        $"{nameof(RateLimitSettings.PermitLimit)} must be greater than 0");
                
                config.PermitLimit = rateLimitSettings.PermitLimit;
                config.Window = TimeSpan.FromSeconds(rateLimitSettings.WindowInSeconds);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = rateLimitSettings.QueueLimit;
            });

            options.OnRejected = async (context, token) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterTimeSpan))
                {
                    var retryAfter = (int)retryAfterTimeSpan.TotalSeconds;
                    context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString();
                }
                
                var problemDetails = ProblemDetailsFactory.CreateTooManyRequestsProblemDetails(
                    Error.TooManyRequests(
                        ApiErrorCodes.TooManyRequests, 
                        "Too many requests. Please try again later."));
                
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, token);
            };
        });

        return services;
    }
    
    
    public static IServiceCollection AddEndpointDefinitions(
        this IServiceCollection services, 
        params Assembly[] assemblies)
    {
        var endpointDefinitions = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IEndpointDefinition).IsAssignableFrom(type) 
                           && type is { IsAbstract: false, IsInterface: false })
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        services.AddSingleton(endpointDefinitions);

        return services;
    }
}