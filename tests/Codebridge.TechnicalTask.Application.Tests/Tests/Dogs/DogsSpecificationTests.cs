using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Dogs;

public class DogsSpecificationTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenInvalidSortParameterProvided()
    {
        // Arrange
        var sortParams = new SortParameters("invalid_property");
        var query = new GetDogsQuery(sortParameters: sortParams);

        // Act
        var action = () => new DogsSpecification(query);

        // Assert
        action.Should()
            .Throw<InvalidOperationException>();
    }
}