namespace Codebridge.TechnicalTask.API.Models.Dogs;

public record CreateDogRequest(
    string? Name, 
    string? Color,
    double? TailLength, 
    double? Weight);