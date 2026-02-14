using FluentAssertions;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for category operations against the Mercury sandbox.
/// </summary>
public class CategoryTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public CategoryTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetCategoriesAsync_ReturnsCategoriesList()
    {
        var categories = await _sandbox.Client.GetCategoriesAsync();

        categories.Should().NotBeNull();
    }
}
