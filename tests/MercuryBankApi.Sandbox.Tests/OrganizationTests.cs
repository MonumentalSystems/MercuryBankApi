using FluentAssertions;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for organization operations against the Mercury sandbox.
/// </summary>
public class OrganizationTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public OrganizationTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetOrganizationAsync_ReturnsOrganization()
    {
        var org = await _sandbox.Client.GetOrganizationAsync();

        org.Should().NotBeNull();
        org.Id.Should().NotBeEmpty();
    }
}
