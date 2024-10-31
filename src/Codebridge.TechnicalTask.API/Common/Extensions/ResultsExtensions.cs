using Codebridge.TechnicalTask.API.Models.Common;

namespace Codebridge.TechnicalTask.API.Common.Extensions;

public static class ResultsExtensions
{
    public static IResult AddPaginationHeaders(
        this IResult result,
        PaginationMetadata metadata)
    {
        return new PaginationHeaderResult(result, metadata);
    }
}