using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Shared.Models;

namespace Codebridge.TechnicalTask.API.Common.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't create a problem details from a success result");
        }
        
        return result.FirstError switch
        {
            {Type: ErrorType.NotFound} => 
                Results.Problem(ProblemDetailsFactory.CreateNotFoundProblemDetails(result.FirstError)),
            
            {Type: ErrorType.Conflict} => 
                Results.Problem(ProblemDetailsFactory.CreateConflictProblemDetails(result.Errors)),
            
            {Type: ErrorType.Validation} => 
                Results.Problem(ProblemDetailsFactory.CreateBadRequestProblemDetails(result.Errors)),
            
            _ => Results.Problem(ProblemDetailsFactory.CreateInternalServerErrorProblemDetails())
        };
    }
    
    public static IResult ToProblemDetails<T>
        (this Result<T> result) => ToProblemDetails(Result.Failure(result.Errors));
}