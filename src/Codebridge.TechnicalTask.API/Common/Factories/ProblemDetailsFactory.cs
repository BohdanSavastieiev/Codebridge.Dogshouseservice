using System.Net;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using static Codebridge.TechnicalTask.API.Common.Constants.ApiConstants;

namespace Codebridge.TechnicalTask.API.Common.Factories;

public static class ProblemDetailsFactory
{
    private static readonly Dictionary<HttpStatusCode, (string Title, string Type)> StatusCodeMapping = new()
    {
        [HttpStatusCode.BadRequest] = (HttpCodeTitles.BadRequest, HttpCodeTypes.BadRequest),
        [HttpStatusCode.NotFound] = (HttpCodeTitles.NotFound, HttpCodeTypes.NotFound),
        [HttpStatusCode.Conflict] = (HttpCodeTitles.Conflict, HttpCodeTypes.Conflict),
        [HttpStatusCode.TooManyRequests] = (HttpCodeTitles.TooManyRequests, HttpCodeTypes.TooManyRequests),
        [HttpStatusCode.ServiceUnavailable] = (HttpCodeTitles.ServiceUnavailable, HttpCodeTypes.ServiceUnavailable),
        [HttpStatusCode.InternalServerError] = (HttpCodeTitles.InternalServerError, HttpCodeTypes.InternalServerError)
    };

    public static ProblemDetails CreateBadRequestProblemDetails(IEnumerable<Error> errors) =>
        CreateProblemDetails(HttpStatusCode.BadRequest, errors);
    public static ProblemDetails CreateBadRequestProblemDetails(Error error) =>
        CreateProblemDetails(HttpStatusCode.BadRequest, error);

    public static ProblemDetails CreateNotFoundProblemDetails(Error error) =>
        CreateProblemDetails(HttpStatusCode.NotFound, error);

    public static ProblemDetails CreateConflictProblemDetails(IEnumerable<Error> errors) =>
        CreateProblemDetails(HttpStatusCode.Conflict, errors);
    public static ProblemDetails CreateConflictProblemDetails(Error error) =>
        CreateProblemDetails(HttpStatusCode.Conflict, error);

    public static ProblemDetails CreateTooManyRequestsProblemDetails(Error error) =>
        CreateProblemDetails(HttpStatusCode.TooManyRequests, error);

    public static ProblemDetails CreateServiceUnavailableProblemDetails(Error error) =>
        CreateProblemDetails(HttpStatusCode.ServiceUnavailable, error);

    public static ProblemDetails CreateInternalServerErrorProblemDetails() =>
        CreateProblemDetails(HttpStatusCode.InternalServerError);

    private static ProblemDetails CreateProblemDetails(
        HttpStatusCode statusCode) =>
        CreateProblemDetails(statusCode, Array.Empty<Error>());
    
    private static ProblemDetails CreateProblemDetails(
        HttpStatusCode statusCode,
        Error error) =>
        CreateProblemDetails(statusCode, [error]);

    private static ProblemDetails CreateProblemDetails(
        HttpStatusCode statusCode,
        IEnumerable<Error> errors)
    {
        var (title, type) = StatusCodeMapping[statusCode];
        
        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Type = type
        };

        var enumerable = errors.ToList();
        if (enumerable.Any())
        {
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                ["errors"] = enumerable.Select(e => new ErrorResponse(e.Code, e.Message))
            };
        }

        return problemDetails;
    }
}