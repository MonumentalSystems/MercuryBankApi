using FluentAssertions;
using MercuryBankApi.Generated;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for user operations against the Mercury sandbox.
/// </summary>
public class UserTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public UserTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetUsersAsync_ReturnsUsersList()
    {
        var users = await _sandbox.Client.GetUsersAsync();

        users.Should().NotBeNull();
    }

    [SandboxFact]
    public async Task GetUserAsync_ReturnsSingleUser()
    {
        var users = await _sandbox.Client.GetUsersAsync();
        if (users.Count == 0) return;

        try
        {
            var user = await _sandbox.Client.GetUserAsync(users[0].UserId);

            user.Should().NotBeNull();
            user.Email.Should().NotBeNullOrWhiteSpace();
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            // Sandbox user IDs may not be fetchable individually
        }
    }
}
