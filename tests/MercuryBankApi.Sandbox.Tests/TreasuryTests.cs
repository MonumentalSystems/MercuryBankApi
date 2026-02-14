using FluentAssertions;
using MercuryBankApi.Generated;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for treasury operations against the Mercury sandbox.
/// </summary>
public class TreasuryTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public TreasuryTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetTreasuryAccountsAsync_ReturnsTreasuryList()
    {
        try
        {
            var accounts = await _sandbox.Client.GetTreasuryAccountsAsync();

            accounts.Should().NotBeNull();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            // Sandbox may not support treasury endpoints
        }
    }

    [SandboxFact]
    public async Task GetTreasuryTransactionsAsync_ReturnsTransactionsList()
    {
        IReadOnlyList<TreasuryAccount> accounts;
        try
        {
            accounts = await _sandbox.Client.GetTreasuryAccountsAsync();
        }
        catch (ApiException ex) when (ex.StatusCode is 403 or 404)
        {
            return;
        }

        if (accounts.Count == 0) return;

        var transactions = await _sandbox.Client.GetTreasuryTransactionsAsync(accounts[0].Id);

        transactions.Should().NotBeNull();
    }
}
