using FluentAssertions;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for account-related operations against the Mercury sandbox.
/// </summary>
public class AccountTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public AccountTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetAccountsAsync_ReturnsSandboxAccounts()
    {
        var accounts = await _sandbox.Client.GetAccountsAsync();

        accounts.Should().NotBeEmpty("sandbox should have pre-loaded accounts");
    }

    [SandboxFact]
    public async Task GetAccountAsync_ReturnsSingleAccount()
    {
        var accounts = await _sandbox.Client.GetAccountsAsync();
        accounts.Should().NotBeEmpty();

        var account = await _sandbox.Client.GetAccountAsync(accounts[0].Id);

        account.Should().NotBeNull();
        account.Id.Should().Be(accounts[0].Id);
        account.Name.Should().NotBeNullOrWhiteSpace();
    }
}
