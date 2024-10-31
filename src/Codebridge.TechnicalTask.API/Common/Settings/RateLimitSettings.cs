namespace Codebridge.TechnicalTask.API.Common.Settings;

public class RateLimitSettings
{
    public int PermitLimit { get; set; }
    public int WindowInSeconds { get; set; }
    public int QueueLimit { get; set; }
}