namespace Codebridge.TechnicalTask.Domain.Common.Constants;

public enum ErrorType
{
    Validation = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    TooManyRequests = 429,
    ServiceUnavailable = 503
}