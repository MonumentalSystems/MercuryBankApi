using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Shared fixture that creates a configured <see cref="IMercuryBankClient"/>
/// pointing at the Mercury sandbox environment.
/// <para>
/// The sandbox API token is resolved from (in priority order):
/// <list type="number">
///   <item>Environment variable <c>MERCURY_SANDBOX_TOKEN</c></item>
///   <item>.NET user secrets key <c>Mercury:ApiToken</c></item>
/// </list>
/// Tests that depend on this fixture are automatically skipped when no
/// token is available from any source.
/// </para>
/// </summary>
public sealed class SandboxFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public SandboxFixture()
    {
        var token = ResolveToken();

        IsConfigured = !string.IsNullOrWhiteSpace(token);

        var services = new ServiceCollection();

        services.AddMercuryBankApi(opts =>
        {
            opts.BaseUrl = MercuryBankOptions.SandboxBaseUrl;
            opts.ApiToken = token ?? string.Empty;
        });

        _provider = services.BuildServiceProvider();
    }

    /// <summary>Whether sandbox credentials are available.</summary>
    public bool IsConfigured { get; }

    /// <summary>Gets the configured sandbox client.</summary>
    public IMercuryBankClient Client =>
        _provider.GetRequiredService<IMercuryBankClient>();

    public void Dispose() => _provider.Dispose();

    /// <summary>
    /// Resolves the sandbox token from environment variables or user secrets.
    /// </summary>
    internal static string? ResolveToken()
    {
        var envToken = Environment.GetEnvironmentVariable("MERCURY_SANDBOX_TOKEN");
        if (!string.IsNullOrWhiteSpace(envToken))
            return envToken;

        try
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<SandboxFixture>()
                .Build();

            return config.GetValue<string>("Mercury:ApiToken");
        }
        catch
        {
            return null;
        }
    }
}
