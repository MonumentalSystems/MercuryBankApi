namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// A <see cref="FactAttribute"/> that skips the test when the
/// <c>MERCURY_SANDBOX_TOKEN</c> environment variable is not set.
/// </summary>
public sealed class SandboxFactAttribute : FactAttribute
{
    public SandboxFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MERCURY_SANDBOX_TOKEN")))
        {
            Skip = "MERCURY_SANDBOX_TOKEN environment variable is not set. " +
                   "Provide a sandbox API token to run integration tests.";
        }
    }
}
