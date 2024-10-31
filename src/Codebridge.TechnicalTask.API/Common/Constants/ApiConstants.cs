namespace Codebridge.TechnicalTask.API.Common.Constants;

public static class ApiConstants
{
    public const string ApiVersionedPath = "";

    public const string GlobalRateLimitGroup = "GlobalLimit";
    
    public const string AllowAllCorsPolicy = "AllowAll";

    public static class HttpCodeTitles
    {
        public const string BadRequest = "Bad Request";
        public const string NotFound = "Not Found";
        public const string Conflict = "Conflict";
        public const string TooManyRequests = "Too Many Requests";
        public const string ServiceUnavailable = "Service Unavailable";
        public const string InternalServerError = "Internal Server Error";
    }
    
    public static class HttpCodeTypes
    {
        public const string BadRequest = "https://datatracker.ietf.org/html/rfc7231#section-6.5.1";
        public const string NotFound = "https://datatracker.ietf.org/html/rfc7231#section-6.5.4";
        public const string Conflict = "https://datatracker.ietf.org/html/rfc7231#section-6.5.8";
        public const string TooManyRequests = "https://datatracker.ietf.org/html/rfc7231#section-6.6.4";
        public const string ServiceUnavailable = "https://datatracker.ietf.org/doc/html/rfc6585#section-4";
        public const string InternalServerError = "https://datatracker.ietf.org/html/rfc7231#section-6.6.1";
    }
    
    public static class HttpHeaders
    {
        public const string TotalCount = "X-Total-Count";
        public const string PageNumber = "X-Page-Number";
        public const string PageSize = "X-Page-Size";
        public const string TotalPages = "X-Total-Pages";
    }
}