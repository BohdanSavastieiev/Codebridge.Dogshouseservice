using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Domain.Dogs.Repositories;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Repositories;

public class DogRepository(ApplicationDbContext dbContext) 
    : BaseRepository<Dog, string>(dbContext), IDogRepository
{
    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.Dogs.AnyAsync(d => d.Name == name, cancellationToken);
    }
}