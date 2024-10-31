using Codebridge.TechnicalTask.Infrastructure.Persistence.Context;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
        await SeedDataAsync(context);
    }

    private static async Task SeedDataAsync(ApplicationDbContext context)
    {
        if (await context.Dogs.AnyAsync())
        {
            return;
        }

        await context.Dogs.AddRangeAsync(DogSeedData.InitialDogs);
        await context.SaveChangesAsync();
    }
}