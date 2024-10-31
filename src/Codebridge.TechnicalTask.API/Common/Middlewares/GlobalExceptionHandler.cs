using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Exceptions;
using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Codebridge.TechnicalTask.API.Common.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;
        _logger.LogError(exception, 
            "Error occurred. TraceId: {TraceId}\nException message: {Message}", 
            traceId,
            exception.Message);
        
        ProblemDetails problemDetails = exception switch
        {
            BadHttpRequestException => ProblemDetailsFactory.CreateBadRequestProblemDetails(
                Error.Validation(ApiErrorCodes.InvalidRequest,
                    "Request was not successfully mapped.")),
            
            ApiConfigurationException => ProblemDetailsFactory.CreateServiceUnavailableProblemDetails(
                Error.ServiceUnavailable(ApiErrorCodes.ConfigurationMissing,
                    "The service is not properly configured and cannot process the request")),
            
            _ => ProblemDetailsFactory.CreateInternalServerErrorProblemDetails()
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}