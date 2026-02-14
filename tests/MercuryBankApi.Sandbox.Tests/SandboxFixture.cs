using Microsoft.Extensions.DependencyInjection;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Shared fixture that creates a configured <see cref="IMercuryBankClient"/>
/// pointing at the Mercury sandbox environment.
/// <para>
/// Required environment variable: <c>MERCURY_SANDBOX_TOKEN</c>.
/// Tests that depend on this fixture are automatically skipped when the
/// token is not set.
/// </para>
/// </summary>
public sealed class SandboxFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public SandboxFixture()
    {
        var token = Environment.GetEnvironmentVariable("MERCURY_SANDBOX_TOKEN");

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
}
