using System.Text.Json;

namespace Codebridge.TechnicalTask.IntegrationTests.Abstractions;

[Collection("Dogs")]
public abstract class BaseIntegrationTest
{
    protected readonly HttpClient HttpClient;
    
    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
    
    protected static JsonSerializerOptions JsonOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };
}