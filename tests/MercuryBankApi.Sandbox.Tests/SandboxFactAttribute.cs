namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// A <see cref="FactAttribute"/> that skips the test when no sandbox API
/// token is available from environment variables or .NET user secrets.
/// </summary>
public sealed class SandboxFactAttribute : FactAttribute
{
    public SandboxFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(SandboxFixture.ResolveToken()))
        {
            Skip = "No Mercury sandbox token found. " +
                   "Set MERCURY_SANDBOX_TOKEN env var or run: " +
                   "dotnet user-secrets set \"Mercury:ApiToken\" \"<token>\" " +
                   "--project tests/MercuryBankApi.Sandbox.Tests";
        }
    }
}
