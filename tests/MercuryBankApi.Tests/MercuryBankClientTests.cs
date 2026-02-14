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

    // ── Accounts ──────────────────────────────────────────────

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
    public async Task GetAccountStatementsAsync_ReturnsStatements()
    {
        var accountId = Guid.NewGuid();
        var statement = new DepositoryAccountStatement
        {
            Id = Guid.NewGuid(),
            AccountNumber = "1234",
            CompanyLegalName = "TestCo",
            DownloadUrl = "https://example.com/stmt.pdf",
            EndDate = "2025-06-30",
            StartDate = "2025-06-01"
        };
        _api.GetAccountStatementsAsync(
            accountId, null, null, null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new DepositoryAccountStatementsPaginatedResponse { Statements = { statement } });

        var result = await _client.GetAccountStatementsAsync(accountId);

        result.Should().HaveCount(1);
        result[0].AccountNumber.Should().Be("1234");
    }

    [Fact]
    public async Task GetAccountCardsAsync_ReturnsCards()
    {
        var accountId = Guid.NewGuid();
        var card = new AccountCard
        {
            CardId = "card-1",
            CreatedAt = "2025-01-01T00:00:00Z",
            LastFourDigits = "4321",
            NameOnCard = "Test User"
        };
        _api.GetAccountCardsAsync(accountId, Arg.Any<CancellationToken>())
            .Returns(new AccountCardsResponse { Cards = { card } });

        var result = await _client.GetAccountCardsAsync(accountId);

        result.Should().HaveCount(1);
        result[0].LastFourDigits.Should().Be("4321");
    }

    // ── Transactions ──────────────────────────────────────────

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

    // ── Categories ────────────────────────────────────────────

    [Fact]
    public async Task GetCategoriesAsync_ReturnsCategories()
    {
        var category = new CategoryData { Id = Guid.NewGuid(), Name = "Payroll" };
        _api.ListCategoriesAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new CategoriesPaginatedResponse { Categories = { category } });

        var result = await _client.GetCategoriesAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Payroll");
    }

    // ── Organization ──────────────────────────────────────────

    [Fact]
    public async Task GetOrganizationAsync_ReturnsOrganization()
    {
        var org = new OrganizationInfo { Id = Guid.NewGuid() };
        _api.GetOrganizationAsync(Arg.Any<CancellationToken>())
            .Returns(new OrganizationResponse { Organization = org });

        var result = await _client.GetOrganizationAsync();

        result.Id.Should().Be(org.Id);
    }

    // ── Users ─────────────────────────────────────────────────

    [Fact]
    public async Task GetUsersAsync_ReturnsUsers()
    {
        var user = new UserDetails { Email = "test@example.com", FirstName = "Test", LastName = "User" };
        _api.GetUsersAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new UsersPaginatedResponse { Users = { user } });

        var result = await _client.GetUsersAsync();

        result.Should().HaveCount(1);
        result[0].Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetUserAsync_ReturnsSingleUser()
    {
        var userId = Guid.NewGuid();
        var user = new UserDetails { Email = "user@example.com", FirstName = "Jane", LastName = "Doe" };
        _api.GetUserAsync(userId, Arg.Any<CancellationToken>()).Returns(user);

        var result = await _client.GetUserAsync(userId);

        result.Email.Should().Be("user@example.com");
    }

    // ── Events ────────────────────────────────────────────────

    [Fact]
    public async Task GetEventsAsync_ReturnsEvents()
    {
        var ev = new ApiEventResponse
        {
            Id = Guid.NewGuid(),
            OccurredAt = "2025-06-01T12:00:00Z",
            OperationType = ApiEventOperationType.Create
        };
        _api.GetEventsAsync(null, null, null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new ApiEventsPaginatedResponse { Events = { ev } });

        var result = await _client.GetEventsAsync();

        result.Should().HaveCount(1);
        result[0].OperationType.Should().Be(ApiEventOperationType.Create);
    }

    [Fact]
    public async Task GetEventAsync_ReturnsSingleEvent()
    {
        var eventId = Guid.NewGuid();
        var ev = new ApiEventResponse
        {
            Id = eventId,
            OccurredAt = "2025-06-01T12:00:00Z",
            OperationType = ApiEventOperationType.Update
        };
        _api.GetEventAsync(eventId, Arg.Any<CancellationToken>()).Returns(ev);

        var result = await _client.GetEventAsync(eventId);

        result.Id.Should().Be(eventId);
    }

    // ── Treasury ──────────────────────────────────────────────

    [Fact]
    public async Task GetTreasuryAccountsAsync_ReturnsTreasuryAccounts()
    {
        var treasury = new TreasuryAccount { Id = Guid.NewGuid(), CreatedAt = "2025-01-01T00:00:00Z" };
        _api.GetTreasuryAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new TreasuryAccountsPaginatedResponse { Accounts = { treasury } });

        var result = await _client.GetTreasuryAccountsAsync();

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(treasury.Id);
    }

    [Fact]
    public async Task GetTreasuryTransactionsAsync_ReturnsTreasuryTransactions()
    {
        var treasuryId = Guid.NewGuid();
        var txn = new TreasuryTxn
        {
            Id = Guid.NewGuid(),
            AccountId = treasuryId,
            Amount = 1000.00,
            Description = "Dividend"
        };
        _api.GetTreasuryTransactionsAsync(treasuryId, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new TreasuryTransactionsResponse { Transactions = { txn } });

        var result = await _client.GetTreasuryTransactionsAsync(treasuryId);

        result.Should().HaveCount(1);
        result[0].Description.Should().Be("Dividend");
    }

    // ── SAFEs ─────────────────────────────────────────────────

    [Fact]
    public async Task GetSafeRequestsAsync_ReturnsSafeRequests()
    {
        var safe = new APISafeRequest { Id = Guid.NewGuid(), DocumentUrl = "https://example.com/safe.pdf" };
        _api.GetSafeRequestsAsync(Arg.Any<CancellationToken>())
            .Returns(new[] { safe });

        var result = await _client.GetSafeRequestsAsync();

        result.Should().HaveCount(1);
        result[0].DocumentUrl.Should().Be("https://example.com/safe.pdf");
    }

    [Fact]
    public async Task GetSafeRequestAsync_ReturnsSingleSafeRequest()
    {
        var safeId = Guid.NewGuid();
        var safe = new APISafeRequest { Id = safeId, DocumentUrl = "https://example.com/safe.pdf" };
        _api.GetSafeRequestAsync(safeId, Arg.Any<CancellationToken>()).Returns(safe);

        var result = await _client.GetSafeRequestAsync(safeId);

        result.Id.Should().Be(safeId);
    }

    // ── AR Customers ──────────────────────────────────────────

    [Fact]
    public async Task GetCustomersAsync_ReturnsCustomers()
    {
        var customer = new ApiV1ArCustomerResponseData
        {
            Id = Guid.NewGuid(),
            Name = "Acme Corp",
            Email = "acme@example.com"
        };
        _api.ListCustomersAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new ApiV1ArCustomerPaginatedResponseData { Customers = { customer } });

        var result = await _client.GetCustomersAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Acme Corp");
    }

    [Fact]
    public async Task GetCustomerAsync_ReturnsSingleCustomer()
    {
        var customerId = Guid.NewGuid();
        var customer = new ApiV1ArCustomerResponseData
        {
            Id = customerId,
            Name = "Acme Corp",
            Email = "acme@example.com"
        };
        _api.GetCustomerAsync(customerId, Arg.Any<CancellationToken>()).Returns(customer);

        var result = await _client.GetCustomerAsync(customerId);

        result.Id.Should().Be(customerId);
        result.Name.Should().Be("Acme Corp");
    }

    [Fact]
    public async Task CreateCustomerAsync_CreatesAndReturnsCustomer()
    {
        var request = new ApiV1ArCustomerCreateRequest();
        var customer = new ApiV1ArCustomerResponseData
        {
            Id = Guid.NewGuid(),
            Name = "New Customer",
            Email = "new@example.com"
        };
        _api.CreateCustomerAsync(request, Arg.Any<CancellationToken>()).Returns(customer);

        var result = await _client.CreateCustomerAsync(request);

        result.Name.Should().Be("New Customer");
    }

    [Fact]
    public async Task UpdateCustomerAsync_UpdatesAndReturnsCustomer()
    {
        var customerId = Guid.NewGuid();
        var request = new ApiV1ArCustomerUpdateRequest();
        var customer = new ApiV1ArCustomerResponseData
        {
            Id = customerId,
            Name = "Updated Customer",
            Email = "updated@example.com"
        };
        _api.UpdateCustomerAsync(customerId, request, Arg.Any<CancellationToken>()).Returns(customer);

        var result = await _client.UpdateCustomerAsync(customerId, request);

        result.Name.Should().Be("Updated Customer");
    }

    [Fact]
    public async Task DeleteCustomerAsync_CallsGeneratedClient()
    {
        var customerId = Guid.NewGuid();

        await _client.DeleteCustomerAsync(customerId);

        await _api.Received(1).DeleteCustomerAsync(customerId, Arg.Any<CancellationToken>());
    }

    // ── AR Invoices ───────────────────────────────────────────

    [Fact]
    public async Task GetInvoicesAsync_ReturnsInvoices()
    {
        var invoice = new ApiV1ArInvoicesData
        {
            Id = Guid.NewGuid(),
            Amount = 500.00,
            CustomerId = Guid.NewGuid()
        };
        _api.ListInvoicesAsync(null, null, null, null, Arg.Any<CancellationToken>())
            .Returns(new ApiV1ArInvoicesPaginatedResponse { Invoices = { invoice } });

        var result = await _client.GetInvoicesAsync();

        result.Should().HaveCount(1);
        result[0].Amount.Should().Be(500.00);
    }

    [Fact]
    public async Task GetInvoiceAsync_ReturnsSingleInvoice()
    {
        var invoiceId = Guid.NewGuid();
        var invoice = new ApiV1ArInvoiceResponse { Id = invoiceId, Amount = 250.00 };
        _api.GetInvoiceAsync(invoiceId, Arg.Any<CancellationToken>()).Returns(invoice);

        var result = await _client.GetInvoiceAsync(invoiceId);

        result.Id.Should().Be(invoiceId);
        result.Amount.Should().Be(250.00);
    }

    [Fact]
    public async Task CreateInvoiceAsync_CreatesAndReturnsInvoice()
    {
        var request = new ApiV1ArInvoiceCreateRequest();
        var invoice = new ApiV1ArInvoiceResponse { Id = Guid.NewGuid(), Amount = 1000.00 };
        _api.CreateInvoiceAsync(request, Arg.Any<CancellationToken>()).Returns(invoice);

        var result = await _client.CreateInvoiceAsync(request);

        result.Amount.Should().Be(1000.00);
    }

    [Fact]
    public async Task UpdateInvoiceAsync_UpdatesAndReturnsInvoice()
    {
        var invoiceId = Guid.NewGuid();
        var request = new ApiV1ArInvoiceUpdateRequest();
        var invoice = new ApiV1ArInvoiceResponse { Id = invoiceId, Amount = 750.00 };
        _api.UpdateInvoiceAsync(invoiceId, request, Arg.Any<CancellationToken>()).Returns(invoice);

        var result = await _client.UpdateInvoiceAsync(invoiceId, request);

        result.Amount.Should().Be(750.00);
    }

    [Fact]
    public async Task CancelInvoiceAsync_CallsGeneratedClient()
    {
        var invoiceId = Guid.NewGuid();

        await _client.CancelInvoiceAsync(invoiceId);

        await _api.Received(1).CancelInvoiceAsync(invoiceId, Arg.Any<CancellationToken>());
    }

    // ── Send Money ────────────────────────────────────────────

    [Fact]
    public async Task RequestSendMoneyAsync_ReturnsApprovalRequest()
    {
        var accountId = Guid.NewGuid();
        var request = new SendMoneyAPIRequest();
        var response = new SendMoneyApprovalRequestResponse { AccountId = accountId, Amount = 100.00 };
        _api.RequestSendMoneyAsync(accountId, request, Arg.Any<CancellationToken>()).Returns(response);

        var result = await _client.RequestSendMoneyAsync(accountId, request);

        result.AccountId.Should().Be(accountId);
        result.Amount.Should().Be(100.00);
    }

    [Fact]
    public async Task GetSendMoneyApprovalRequestAsync_ReturnsApprovalRequest()
    {
        var requestId = Guid.NewGuid();
        var response = new SendMoneyApprovalRequestResponse
        {
            AccountId = Guid.NewGuid(),
            Amount = 200.00
        };
        _api.GetSendMoneyApprovalRequestAsync(requestId, Arg.Any<CancellationToken>()).Returns(response);

        var result = await _client.GetSendMoneyApprovalRequestAsync(requestId);

        result.Amount.Should().Be(200.00);
    }
}
