namespace Codebridge.TechnicalTask.Domain.Shared.Models;

public class Result
{
    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        if (isSuccess && errors.Count > 0)
            throw new InvalidOperationException();
        
        if (!isSuccess && errors.Count == 0)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }
    public Error FirstError => Errors[0];

    public static Result Success() => 
        new(true, Array.Empty<Error>());
    public static Result<TValue> Success<TValue>(TValue value) => 
        new(value, true, Array.Empty<Error>());

    public static Result Failure(Error error) => 
        new(false, [error]);
    
    public static Result Failure(IEnumerable<Error> errors) => 
        new(false, errors.ToList());
    
    public static Result<TValue> Failure<TValue>(Error error) => 
        new(default, false, [error]);
    
    public static Result<TValue> Failure<TValue>(IEnumerable<Error> errors) => 
        new(default, false, errors.ToList());
}