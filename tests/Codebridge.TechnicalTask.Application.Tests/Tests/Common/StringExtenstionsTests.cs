using Codebridge.TechnicalTask.Application.Common.Extensions;

namespace Codebridge.TechnicalTask.Application.Tests.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("HelloWorld", "hello_world")]
    [InlineData("ABC", "a_b_c")]
    [InlineData("myPropertyName", "my_property_name")]
    [InlineData("", "")]
    [InlineData("already_snake_case", "already_snake_case")]
    [InlineData("A%$#BC#132D", "a%$#_b_c#132_d")]
    public void ToLowerSnakeCase_ShouldConvertCorrectly(string input, string expected)
    {
        var result = input.ToLowerSnakeCase();
        Assert.Equal(expected, result);
    }
}