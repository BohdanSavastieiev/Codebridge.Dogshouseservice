using System.Threading.RateLimiting;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Exceptions;
using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.API.Common.Settings;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using Microsoft.AspNetCore.RateLimiting;

namespace Codebridge.TechnicalTask.API.Common.Extensions.Configuration;

public static class RateLimitingExtensions 
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(RateLimitSettings))
                           .Get<RateLimitSettings>()
                       ?? throw new ApiConfigurationException(nameof(RateLimitSettings));

        services.Configure<RateLimitSettings>(configuration.GetSection(nameof(RateLimitSettings)));
        
        services.AddRateLimiter(options => ConfigureRateLimiting(options, settings));

        return services;
    }

    private static void ConfigureRateLimiting(RateLimiterOptions options, RateLimitSettings settings)
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddFixedWindowLimiter(ApiConstants.GlobalRateLimitGroup, config => 
            ConfigureFixedWindow(config, settings));
        options.OnRejected = HandleRateLimitRejection;
    }

    private static void ConfigureFixedWindow(FixedWindowRateLimiterOptions config, RateLimitSettings settings)
    {
        if (settings.PermitLimit <= 0)
            throw new ApiConfigurationException(
                $"{nameof(RateLimitSettings.PermitLimit)} must be greater than 0");

        config.PermitLimit = settings.PermitLimit;
        config.Window = TimeSpan.FromSeconds(settings.WindowInSeconds);
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = settings.QueueLimit;
    }
    
    private static async ValueTask HandleRateLimitRejection(OnRejectedContext context, CancellationToken token)
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
    }
}