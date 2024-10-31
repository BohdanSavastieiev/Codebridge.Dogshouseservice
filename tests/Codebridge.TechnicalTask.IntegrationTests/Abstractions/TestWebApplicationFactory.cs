using System.Text.Json;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.Infrastructure;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Testcontainers.MsSql;

namespace Codebridge.TechnicalTask.IntegrationTests.Abstractions;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    
    public TestWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Admin123!")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithName($"test-dogservice-db-{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var initialData = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
            };

            configBuilder.AddInMemoryCollection(initialData);
        });
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
            
            services.RemoveAll<IConfigureOptions<RateLimiterOptions>>();
            
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(ApiConstants.GlobalRateLimitGroup, config =>
                {
                    config.PermitLimit = 1000000;
                    config.Window = TimeSpan.FromSeconds(1);
                    config.QueueLimit = 0;
                });
            });
            
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });
        });
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await using var scope = Services.CreateAsyncScope();
        await scope.ServiceProvider.InitializeDatabaseAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}