using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Interfaces;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Extensions;
using Serilog;

namespace Codebridge.TechnicalTask.API.Common.Extensions.Configuration;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler()
            .UseCors(ApiConstants.AllowAllCorsPolicy)
            .UseHttpsRedirection()
            .UseSerilogRequestLogging()
            .UseRateLimiter();

        return app;
    }

    public static async Task ConfigureEndpoints(this WebApplication app)
    {
        await app.Services.InitializeDatabaseAsync();

        var group = app.MapGroup("/")
            .RequireRateLimiting(ApiConstants.GlobalRateLimitGroup);

        var definitions = app.Services.GetRequiredService<IEnumerable<IEndpointDefinition>>();
        foreach (var definition in definitions)
        {
            definition.MapEndpoints(group);
        }
    }
}