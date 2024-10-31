namespace Codebridge.TechnicalTask.Domain.Common.Exceptions;

public class InvalidDomainException : Exception
{
    public InvalidDomainException(string message) : base(message)
    {
    }
}