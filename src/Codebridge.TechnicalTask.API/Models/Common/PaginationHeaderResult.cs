using Codebridge.TechnicalTask.API.Common.Constants;

namespace Codebridge.TechnicalTask.API.Models.Common;

public class PaginationHeaderResult : IResult
{
    private readonly IResult _result;
    private readonly PaginationMetadata _metadata;

    public PaginationHeaderResult(IResult result, PaginationMetadata metadata)
    {
        _result = result;
        _metadata = metadata;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.Append(ApiConstants.HttpHeaders.PageNumber, _metadata.PageNumber.ToString());
        httpContext.Response.Headers.Append(ApiConstants.HttpHeaders.PageSize, _metadata.PageSize.ToString());
        httpContext.Response.Headers.Append(ApiConstants.HttpHeaders.TotalPages, _metadata.TotalPages.ToString());
        httpContext.Response.Headers.Append(ApiConstants.HttpHeaders.TotalCount, _metadata.TotalCount.ToString());

        await _result.ExecuteAsync(httpContext);
    }
}