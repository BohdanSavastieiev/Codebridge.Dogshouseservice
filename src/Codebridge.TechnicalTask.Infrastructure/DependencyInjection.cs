using Codebridge.TechnicalTask.Domain.Dogs.Repositories;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Context;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Extensions;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codebridge.TechnicalTask.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            options.UseSqlServer(connectionString, sqlOptions => 
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.MigrationsHistoryTable("__ef_migration_history");
            });
        });

        services.AddScoped<IDogRepository, DogRepository>();

        return services;
    }
    
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await DatabaseExtensions.InitializeDatabaseAsync(serviceProvider);    
    }
}