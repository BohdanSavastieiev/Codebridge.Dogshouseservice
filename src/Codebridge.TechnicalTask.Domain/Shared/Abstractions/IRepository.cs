namespace Codebridge.TechnicalTask.Domain.Shared.Abstractions;

public interface IRepository<T, TId> 
    where T : class
    where TId : IEquatable<TId>
{
    Task<T?> FindAsync(TId id, CancellationToken cancellationToken = default);
    Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}