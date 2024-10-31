using System.Reflection;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.UseSnakeCase();
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}