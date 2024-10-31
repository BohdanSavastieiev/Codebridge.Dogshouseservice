using System.Text.RegularExpressions;

namespace Codebridge.TechnicalTask.Application.Common.Extensions;

public static class StringExtensions
{
    public static string? ToLowerSnakeCase(this string? str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        var pattern = "(?<!^)(?=[A-Z])";
        return Regex.Replace(str, pattern, "_").ToLower();
    }
}