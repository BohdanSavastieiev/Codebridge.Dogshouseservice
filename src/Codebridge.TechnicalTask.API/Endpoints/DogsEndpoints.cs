using AutoMapper;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Extensions;
using Codebridge.TechnicalTask.API.Common.Interfaces;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.API.Models.Dogs;
using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;
using Codebridge.TechnicalTask.Application.Dogs.Queries.GetDog;
using Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.API.Endpoints;

public class DogsEndpoints : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup($"{ApiConstants.ApiVersionedPath}/dogs");

        group.MapGet("/", GetDogs)
            .WithValidation<PaginationRequest>()
            .WithValidation<SortRequest>();
        
        group.MapGet("/{name}", GetDog)
            .WithName(nameof(GetDog));
        
        group.MapPost("/", CreateDog)
            .WithValidation<CreateDogRequest>();
    }

    private static async Task<IResult> GetDogs(
        [AsParameters] PaginationRequest pagination,
        [AsParameters] SortRequest sort,
        ISender sender,
        CancellationToken cancellationToken)
    {
       var paginationParameters = new PaginationParameters(
            pageSize: pagination.PageSize,
            pageNumber: pagination.PageNumber);
       
       SortParameters? sortParameters = null;
       if (!string.IsNullOrWhiteSpace(sort.Attribute))
       {
           var sortResult = SortParameters.Create(sort.Attribute!, sort.Order);
           if (sortResult.IsFailure)
           {
               throw new InvalidOperationException("Invalid sort state.");
           }
           
           sortParameters = sortResult.Value;
       }
        
       var query = new GetDogsQuery(paginationParameters, sortParameters);
       var result = await sender.Send(query, cancellationToken);

       if (!result.IsSuccess)
       {
           return result.ToProblemDetails();
       }
        
       var metadata = new PaginationMetadata(
           result.Value.PageNumber,
           result.Value.PageSize,
           result.Value.TotalPages,
           result.Value.TotalCount);

       return TypedResults.Ok(result.Value.Items)
           .AddPaginationHeaders(metadata);
    }

    private static async Task<IResult> GetDog(
        string name,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDogQuery(name), cancellationToken);

        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblemDetails();
    }

    private static async Task<IResult> CreateDog(
        CreateDogRequest request,
        ISender sender,
        IMapper mapper,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateDogCommand>(request);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.ToProblemDetails();

        var url = linkGenerator.GetUriByName(
            httpContext,
            nameof(GetDog),
            new { name = result.Value.Name })!;
            
        return Results.Created(url, result.Value);
    }
}