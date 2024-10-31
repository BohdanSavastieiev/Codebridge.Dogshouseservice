using Codebridge.TechnicalTask.Domain.Common.Constants;

namespace Codebridge.TechnicalTask.Domain.Shared.Models;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error NotFound(string code, string message) => 
        new(code, message, ErrorType.NotFound);
        
    public static Error Validation(string code, string message) => 
        new(code, message, ErrorType.Validation);
        
    public static Error Conflict(string code, string message) => 
        new(code, message, ErrorType.Conflict);
        
    public static Error Unauthorized(string code, string message) => 
        new(code, message, ErrorType.Unauthorized);
    
    public static Error ServiceUnavailable(string code, string message) => 
        new(code, message, ErrorType.ServiceUnavailable);
    
    public static Error TooManyRequests(string code, string message) => 
        new(code, message, ErrorType.TooManyRequests);
}