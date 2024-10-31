using Codebridge.TechnicalTask.Application.Common.Contracts;
using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Application.Dogs.Common;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;

public record GetDogsQuery : IQuery<PaginatedList<DogDto>>
{
    public PaginationParameters PaginationParameters { get; }
    public SortParameters? SortParameters { get; }

    public GetDogsQuery(
        PaginationParameters? paginationParameters = default,
        SortParameters? sortParameters = default)
    {
        PaginationParameters = paginationParameters ?? new PaginationParameters();
        SortParameters = sortParameters;
    }
}