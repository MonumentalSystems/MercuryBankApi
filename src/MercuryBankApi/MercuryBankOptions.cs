namespace MercuryBankApi;

/// <summary>
/// Configuration options for the Mercury Bank API client.
/// </summary>
public sealed class MercuryBankOptions
{
    /// <summary>Configuration section name for binding.</summary>
    public const string SectionName = "Mercury";

    /// <summary>Production API base URL.</summary>
    public const string ProductionBaseUrl = "https://api.mercury.com/api/v1";

    /// <summary>Sandbox API base URL for testing.</summary>
    public const string SandboxBaseUrl = "https://api-sandbox.mercury.com/api/v1";

    /// <summary>Mercury API base URL. Defaults to production.</summary>
    public string BaseUrl { get; set; } = ProductionBaseUrl;

    /// <summary>Mercury API bearer token.</summary>
    public string ApiToken { get; set; } = string.Empty;
}
