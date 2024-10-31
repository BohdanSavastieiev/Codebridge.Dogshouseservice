namespace Codebridge.TechnicalTask.API.Common.Exceptions;

public class ApiConfigurationException : Exception
{
    public ApiConfigurationException(string configurationSection) 
        : base($"Missing configuration section: {configurationSection}")
    {
    }
}