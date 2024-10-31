using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Common.Interfaces;
using Codebridge.TechnicalTask.API.Common.Settings;
using Microsoft.Extensions.Options;

namespace Codebridge.TechnicalTask.API.Endpoints;

public class PingEndpoints : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.ApiVersionedPath}/ping", (IOptions<ServiceSettings> settings) => 
            $"{settings.Value.Name}.Version{settings.Value.Version}");
    }
}