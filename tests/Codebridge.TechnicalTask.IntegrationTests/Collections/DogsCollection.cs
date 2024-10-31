using Codebridge.TechnicalTask.IntegrationTests.Abstractions;

namespace Codebridge.TechnicalTask.IntegrationTests.Collections;

[CollectionDefinition("Dogs")]
public class DogsCollection : ICollectionFixture<TestWebApplicationFactory>;