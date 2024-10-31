using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Domain.Shared.Abstractions;

namespace Codebridge.TechnicalTask.Domain.Dogs.Repositories;

public interface IDogRepository : IRepository<Dog, string>
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);
}