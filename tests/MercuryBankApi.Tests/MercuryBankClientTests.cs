using FluentAssertions;
using MercuryBankApi.Generated;
using NSubstitute;

namespace MercuryBankApi.Tests;

public class MercuryBankClientTests
{
    private readonly IMercuryApiClient _api = Substitute.For<IMercuryApiClient>();
    private readonly MercuryBankClient _client;

    public MercuryBankClientTests()
    {
        _client = new MercuryBankClient(_api);
    }

    [Fact]
    public async Task GetAccountsAsync_ReturnsAccounts()
    {
        var account = new Account { Id = Guid.NewGuid(), Name = "Checking" };
        _api.GetAccountsAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new AccountsPaginatedResponse { Accounts = { account } });

        var result = await _client.GetAccountsAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Checking");
    }

    [Fact]
    public async Task GetAccountAsync_ReturnsAccount()
    {
        var id = Guid.NewGuid();
        var account = new Account { Id = id, Name = "Savings" };
        _api.GetAccountAsync(id, Arg.Any<CancellationToken>()).Returns(account);

        var result = await _client.GetAccountAsync(id);

        result.Name.Should().Be("Savings");
    }

    [Fact]
    public async Task GetTransactionsAsync_ReturnsTransactions()
    {
        var tx = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = -100.50,
            CreatedAt = "2025-06-01T12:00:00Z",
            Status = TransactionStatus.Sent,
            CounterpartyName = "Vendor",
            Kind = TransactionKind.ExternalTransfer
        };
        _api.ListTransactionsAsync(
            null, null, "2025-06-01", "2025-06-30",
            null, null, null, null, null, null, null, null, null, null,
            Arg.Any<CancellationToken>())
            .Returns(new TransactionsPaginatedResponse { Transactions = { tx } });

        var result = await _client.GetTransactionsAsync(
            new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));

        result.Should().HaveCount(1);
        result[0].Amount.Should().Be(-100.50);
        result[0].CounterpartyName.Should().Be("Vendor");
    }

    [Fact]
    public async Task GetAccountTransactionsAsync_ReturnsTransactions()
    {
        var accountId = Guid.NewGuid();
        var tx = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 500.00,
            CreatedAt = "2025-06-15T12:00:00Z",
            Status = TransactionStatus.Pending,
            CounterpartyName = "Client",
            Kind = TransactionKind.IncomingDomesticWire
        };
        _api.ListAccountTransactionsAsync(
            accountId, null, "2025-06-01", "2025-06-30",
            null, null, null, null, null, null, null,
            Arg.Any<CancellationToken>())
            .Returns(new TransactionsResponse { Transactions = { tx } });

        var result = await _client.GetAccountTransactionsAsync(
            accountId, new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));

        result.Should().HaveCount(1);
        result[0].Status.Should().Be(TransactionStatus.Pending);
    }

    [Fact]
    public async Task GetTransactionAsync_ReturnsSingleTransaction()
    {
        var txId = Guid.NewGuid();
        var tx = new Transaction
        {
            Id = txId,
            Amount = 42.00,
            CreatedAt = "2025-07-01T12:00:00Z",
            Status = TransactionStatus.Sent,
            CounterpartyName = "Someone",
            Kind = TransactionKind.OutgoingPayment
        };
        _api.GetTransactionByIdAsync(txId, Arg.Any<CancellationToken>()).Returns(tx);

        var result = await _client.GetTransactionAsync(txId);

        result.Id.Should().Be(txId);
        result.Amount.Should().Be(42.00);
    }
}
