namespace Codebridge.TechnicalTask.API.Common.Interfaces;

public interface IEndpointDefinition
{
    void MapEndpoints(IEndpointRouteBuilder app);
}