using MercuryBankApi.Generated;

namespace MercuryBankApi;

/// <summary>
/// High-level client for the Mercury Bank API.
/// Wraps the NSwag-generated client with a simplified interface.
/// </summary>
public sealed class MercuryBankClient : IMercuryBankClient
{
    private readonly IMercuryApiClient _api;

    /// <summary>Creates a new Mercury Bank client wrapping the generated API client.</summary>
    public MercuryBankClient(IMercuryApiClient api)
    {
        _api = api;
    }

    // ── Accounts ──────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<Account>> GetAccountsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetAccountsAsync(
            limit: null, order: null, start_after: null, end_before: null, cancellationToken);
        return response.Accounts.ToList();
    }

    /// <inheritdoc />
    public async Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _api.GetAccountAsync(accountId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DepositoryAccountStatement>> GetAccountStatementsAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var response = await _api.GetAccountStatementsAsync(
            accountId, limit: null, order: null, start_after: null, end_before: null,
            start: null, end: null, cancellationToken);
        return response.Statements.ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AccountCard>> GetAccountCardsAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var response = await _api.GetAccountCardsAsync(accountId, cancellationToken);
        return response.Cards.ToList();
    }

    // ── Transactions ──────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<Transaction>> GetTransactionsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await _api.ListTransactionsAsync(
            status: null,
            search: null,
            start: from.ToString("yyyy-MM-dd"),
            end: to.ToString("yyyy-MM-dd"),
            postedStart: null,
            postedEnd: null,
            accountId: null,
            mercuryCategory: null,
            categoryId: null,
            start_at: null,
            start_after: null,
            end_before: null,
            limit: null,
            order: null,
            cancellationToken);
        return response.Transactions.ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Transaction>> GetAccountTransactionsAsync(Guid accountId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await _api.ListAccountTransactionsAsync(
            accountId,
            limit: null,
            start: from.ToString("yyyy-MM-dd"),
            end: to.ToString("yyyy-MM-dd"),
            search: null,
            status: null,
            offset: null,
            order: null,
            requestId: null,
            mercuryCategory: null,
            categoryId: null,
            cancellationToken);
        return response.Transactions.ToList();
    }

    /// <inheritdoc />
    public async Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await _api.GetTransactionByIdAsync(transactionId, cancellationToken);
    }

    // ── Categories ────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<CategoryData>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.ListCategoriesAsync(
            limit: null, order: null, start_after: null, end_before: null, cancellationToken);
        return response.Categories.ToList();
    }

    // ── Organization ──────────────────────────────────────────

    /// <inheritdoc />
    public async Task<OrganizationInfo> GetOrganizationAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetOrganizationAsync(cancellationToken);
        return response.Organization;
    }

    // ── Users ─────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserDetails>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetUsersAsync(
            limit: null, start_after: null, end_before: null, order: null, cancellationToken);
        return response.Users.ToList();
    }

    /// <inheritdoc />
    public async Task<UserDetails> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _api.GetUserAsync(userId, cancellationToken);
    }

    // ── Events ────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<ApiEventResponse>> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetEventsAsync(
            limit: null, start_after: null, end_before: null, order: null,
            resourceType: null, resourceId: null, cancellationToken);
        return response.Events.ToList();
    }

    /// <inheritdoc />
    public async Task<ApiEventResponse> GetEventAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _api.GetEventAsync(eventId, cancellationToken);
    }

    // ── Treasury ──────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<TreasuryAccount>> GetTreasuryAccountsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetTreasuryAsync(
            limit: null, start_after: null, end_before: null, order: null, cancellationToken);
        return response.Accounts.ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TreasuryTxn>> GetTreasuryTransactionsAsync(Guid treasuryId, CancellationToken cancellationToken = default)
    {
        var response = await _api.GetTreasuryTransactionsAsync(
            treasuryId, limit: null, order: null, cursor: null, cancellationToken);
        return response.Transactions.ToList();
    }

    // ── SAFEs ─────────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<APISafeRequest>> GetSafeRequestsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.GetSafeRequestsAsync(cancellationToken);
        return response.ToList();
    }

    /// <inheritdoc />
    public async Task<APISafeRequest> GetSafeRequestAsync(Guid safeRequestId, CancellationToken cancellationToken = default)
    {
        return await _api.GetSafeRequestAsync(safeRequestId, cancellationToken);
    }

    // ── AR Customers ──────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<ApiV1ArCustomerResponseData>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.ListCustomersAsync(
            limit: null, start_after: null, end_before: null, order: null, cancellationToken);
        return response.Customers.ToList();
    }

    /// <inheritdoc />
    public async Task<ApiV1ArCustomerResponseData> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _api.GetCustomerAsync(customerId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiV1ArCustomerResponseData> CreateCustomerAsync(ApiV1ArCustomerCreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _api.CreateCustomerAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiV1ArCustomerResponseData> UpdateCustomerAsync(Guid customerId, ApiV1ArCustomerUpdateRequest request, CancellationToken cancellationToken = default)
    {
        return await _api.UpdateCustomerAsync(customerId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        await _api.DeleteCustomerAsync(customerId, cancellationToken);
    }

    // ── AR Invoices ───────────────────────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<ApiV1ArInvoicesData>> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _api.ListInvoicesAsync(
            limit: null, order: null, start_after: null, end_before: null, cancellationToken);
        return response.Invoices.ToList();
    }

    /// <inheritdoc />
    public async Task<ApiV1ArInvoiceResponse> GetInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        return await _api.GetInvoiceAsync(invoiceId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiV1ArInvoiceResponse> CreateInvoiceAsync(ApiV1ArInvoiceCreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _api.CreateInvoiceAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiV1ArInvoiceResponse> UpdateInvoiceAsync(Guid invoiceId, ApiV1ArInvoiceUpdateRequest request, CancellationToken cancellationToken = default)
    {
        return await _api.UpdateInvoiceAsync(invoiceId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task CancelInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        await _api.CancelInvoiceAsync(invoiceId, cancellationToken);
    }

    // ── Send Money ────────────────────────────────────────────

    /// <inheritdoc />
    public async Task<SendMoneyApprovalRequestResponse> RequestSendMoneyAsync(Guid accountId, SendMoneyAPIRequest request, CancellationToken cancellationToken = default)
    {
        return await _api.RequestSendMoneyAsync(accountId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SendMoneyApprovalRequestResponse> GetSendMoneyApprovalRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _api.GetSendMoneyApprovalRequestAsync(requestId, cancellationToken);
    }
}
