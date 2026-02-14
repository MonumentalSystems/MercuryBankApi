using FluentAssertions;

namespace MercuryBankApi.Sandbox.Tests;

/// <summary>
/// Integration tests for transaction-related operations against the Mercury sandbox.
/// </summary>
public class TransactionTests : IClassFixture<SandboxFixture>
{
    private readonly SandboxFixture _sandbox;

    public TransactionTests(SandboxFixture sandbox)
    {
        _sandbox = sandbox;
    }

    [SandboxFact]
    public async Task GetTransactionsAsync_ReturnsSandboxTransactions()
    {
        // Use a wide date range to capture sandbox dummy data.
        var from = new DateOnly(2020, 1, 1);
        var to = DateOnly.FromDateTime(DateTime.UtcNow);

        var transactions = await _sandbox.Client.GetTransactionsAsync(from, to);

        transactions.Should().NotBeEmpty("sandbox should have pre-loaded transactions");
    }

    [SandboxFact]
    public async Task GetAccountTransactionsAsync_ReturnsSandboxTransactions()
    {
        var accounts = await _sandbox.Client.GetAccountsAsync();
        accounts.Should().NotBeEmpty();

        var from = new DateOnly(2020, 1, 1);
        var to = DateOnly.FromDateTime(DateTime.UtcNow);

        var transactions = await _sandbox.Client.GetAccountTransactionsAsync(
            accounts[0].Id, from, to);

        transactions.Should().NotBeNull();
        // Sandbox account may or may not have transactions; just verify no errors.
    }

    [SandboxFact]
    public async Task GetTransactionAsync_ReturnsSingleTransaction()
    {
        var from = new DateOnly(2020, 1, 1);
        var to = DateOnly.FromDateTime(DateTime.UtcNow);

        var transactions = await _sandbox.Client.GetTransactionsAsync(from, to);
        transactions.Should().NotBeEmpty("need at least one transaction to test single fetch");

        var transaction = await _sandbox.Client.GetTransactionAsync(transactions[0].Id);

        transaction.Should().NotBeNull();
        transaction.Id.Should().Be(transactions[0].Id);
    }
}
