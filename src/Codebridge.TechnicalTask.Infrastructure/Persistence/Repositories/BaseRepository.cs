using Codebridge.TechnicalTask.Domain.Shared.Abstractions;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Context;
using Codebridge.TechnicalTask.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Repositories;


public class BaseRepository<T, TId>(ApplicationDbContext dbContext) : IRepository<T, TId>
    where T : class
    where TId : IEquatable<TId>
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public virtual async Task<T?> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .FindAsync([id], cancellationToken)
            .AsTask();
    }

    public virtual async Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken)
    {
        var query = ApplySpecification(spec);
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return entity;
    }
    
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().CountAsync(cancellationToken);
    }
    
    protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
    }
}