using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Common.Exceptions;

namespace Codebridge.TechnicalTask.Domain.Dogs.Entities;

public class Dog
{ 
    public string Name { get; set; }
    public string Color { get; set; }
    public double TailLength { get; set; }
    public double Weight { get; set; }

    public Dog(string name, string color, double tailLength, double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDomainException("Dog must have a name.");
        
        if (name.Length > DomainConstants.Dog.MaxNameLength)
            throw new InvalidDomainException($"Dog's name should not exceed {DomainConstants.Dog.MaxNameLength} characters.");
            
        if (string.IsNullOrWhiteSpace(color))
            throw new InvalidDomainException("Dog must have a color.");
        
        if (color.Length > DomainConstants.Dog.MaxColorLength)
            throw new InvalidDomainException($"Dog's color should not exceed {DomainConstants.Dog.MaxColorLength} characters.");
            
        if (tailLength < 0)
            throw new InvalidDomainException("Tail length must be zero or greater.");
            
        if (weight <= 0)
            throw new InvalidDomainException("Weight must be greater than zero.");
        
        Name = name;
        Color = color;
        TailLength = tailLength;
        Weight = weight;
    }
}