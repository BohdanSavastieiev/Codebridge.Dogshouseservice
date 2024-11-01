using System.Linq.Expressions;

namespace Codebridge.TechnicalTask.Domain.Shared.Abstractions;

public interface ISpecification<T>
{
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPaginationEnabled { get; }
}