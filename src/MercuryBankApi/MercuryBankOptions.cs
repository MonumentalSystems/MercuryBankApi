namespace MercuryBankApi;

/// <summary>
/// Configuration options for the Mercury Bank API client.
/// </summary>
public sealed class MercuryBankOptions
{
    /// <summary>Configuration section name for binding.</summary>
    public const string SectionName = "Mercury";

    /// <summary>Mercury API base URL. Defaults to production.</summary>
    public string BaseUrl { get; set; } = "https://api.mercury.com/api/v1";

    /// <summary>Mercury API bearer token.</summary>
    public string ApiToken { get; set; } = string.Empty;
}
