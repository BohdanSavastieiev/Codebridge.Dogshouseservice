using Codebridge.TechnicalTask.Application.Common.Constants;

namespace Codebridge.TechnicalTask.Application.Common.Models;

public record PaginationParameters
{
    public int PageSize { get; } = ApplicationConstants.Pagination.DefaultPageSize;
    public int PageNumber { get; } = ApplicationConstants.Pagination.DefaultPageNumber;

    public PaginationParameters() { }
    public PaginationParameters(int? pageSize, int? pageNumber)
    {
        if (pageSize is > 0 and < ApplicationConstants.Pagination.MaxPageSize)
        {
            PageSize = pageSize.Value;
        }
        else if (pageSize is > ApplicationConstants.Pagination.MaxPageSize)
        {
            PageSize = ApplicationConstants.Pagination.MaxPageSize;
        }

        if (pageNumber is > 0)
        {
            PageNumber = pageNumber.Value;
        }
    }
}