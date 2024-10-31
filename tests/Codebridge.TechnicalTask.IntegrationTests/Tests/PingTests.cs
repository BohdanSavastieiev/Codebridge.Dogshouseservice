using System.Net;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Codebridge.TechnicalTask.IntegrationTests.Tests;

public class PingTests : BaseIntegrationTest
{
    public PingTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnOk_WithServiceInfo()
    {
        // Act
        var response = await HttpClient.GetAsync($"{ApiConstants.ApiVersionedPath}/ping");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}