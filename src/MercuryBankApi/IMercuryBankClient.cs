using MercuryBankApi.Generated;

namespace MercuryBankApi;

/// <summary>
/// High-level client for the Mercury Bank API.
/// </summary>
public interface IMercuryBankClient
{
    // ── Accounts ──────────────────────────────────────────────

    /// <summary>Lists all accounts.</summary>
    Task<IReadOnlyList<Account>> GetAccountsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single account by ID.</summary>
    Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>Lists statements for a specific account.</summary>
    Task<IReadOnlyList<DepositoryAccountStatement>> GetAccountStatementsAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>Lists cards for a specific account.</summary>
    Task<IReadOnlyList<AccountCard>> GetAccountCardsAsync(Guid accountId, CancellationToken cancellationToken = default);

    // ── Transactions ──────────────────────────────────────────

    /// <summary>Lists transactions across all accounts for the given date range.</summary>
    Task<IReadOnlyList<Transaction>> GetTransactionsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default);

    /// <summary>Lists transactions for a specific account.</summary>
    Task<IReadOnlyList<Transaction>> GetAccountTransactionsAsync(Guid accountId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);

    /// <summary>Gets a single transaction by ID.</summary>
    Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);

    // ── Categories ────────────────────────────────────────────

    /// <summary>Lists all categories.</summary>
    Task<IReadOnlyList<CategoryData>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    // ── Organization ──────────────────────────────────────────

    /// <summary>Gets the organization details.</summary>
    Task<OrganizationInfo> GetOrganizationAsync(CancellationToken cancellationToken = default);

    // ── Users ─────────────────────────────────────────────────

    /// <summary>Lists all users in the organization.</summary>
    Task<IReadOnlyList<UserDetails>> GetUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single user by ID.</summary>
    Task<UserDetails> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);

    // ── Events ────────────────────────────────────────────────

    /// <summary>Lists all events.</summary>
    Task<IReadOnlyList<ApiEventResponse>> GetEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single event by ID.</summary>
    Task<ApiEventResponse> GetEventAsync(Guid eventId, CancellationToken cancellationToken = default);

    // ── Treasury ──────────────────────────────────────────────

    /// <summary>Lists all treasury accounts.</summary>
    Task<IReadOnlyList<TreasuryAccount>> GetTreasuryAccountsAsync(CancellationToken cancellationToken = default);

    /// <summary>Lists transactions for a specific treasury account.</summary>
    Task<IReadOnlyList<TreasuryTxn>> GetTreasuryTransactionsAsync(Guid treasuryId, CancellationToken cancellationToken = default);

    // ── SAFEs ─────────────────────────────────────────────────

    /// <summary>Lists all SAFE requests.</summary>
    Task<IReadOnlyList<APISafeRequest>> GetSafeRequestsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single SAFE request by ID.</summary>
    Task<APISafeRequest> GetSafeRequestAsync(Guid safeRequestId, CancellationToken cancellationToken = default);

    // ── AR Customers ──────────────────────────────────────────

    /// <summary>Lists all accounts receivable customers.</summary>
    Task<IReadOnlyList<ApiV1ArCustomerResponseData>> GetCustomersAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single accounts receivable customer by ID.</summary>
    Task<ApiV1ArCustomerResponseData> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new accounts receivable customer.</summary>
    Task<ApiV1ArCustomerResponseData> CreateCustomerAsync(ApiV1ArCustomerCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing accounts receivable customer.</summary>
    Task<ApiV1ArCustomerResponseData> UpdateCustomerAsync(Guid customerId, ApiV1ArCustomerUpdateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Deletes an accounts receivable customer.</summary>
    Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);

    // ── AR Invoices ───────────────────────────────────────────

    /// <summary>Lists all accounts receivable invoices.</summary>
    Task<IReadOnlyList<ApiV1ArInvoicesData>> GetInvoicesAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single accounts receivable invoice by ID.</summary>
    Task<ApiV1ArInvoiceResponse> GetInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new accounts receivable invoice.</summary>
    Task<ApiV1ArInvoiceResponse> CreateInvoiceAsync(ApiV1ArInvoiceCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing accounts receivable invoice.</summary>
    Task<ApiV1ArInvoiceResponse> UpdateInvoiceAsync(Guid invoiceId, ApiV1ArInvoiceUpdateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Cancels an accounts receivable invoice.</summary>
    Task CancelInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default);

    // ── Send Money ────────────────────────────────────────────

    /// <summary>Requests approval to send money from an account.</summary>
    Task<SendMoneyApprovalRequestResponse> RequestSendMoneyAsync(Guid accountId, SendMoneyAPIRequest request, CancellationToken cancellationToken = default);

    /// <summary>Gets a send-money approval request by ID.</summary>
    Task<SendMoneyApprovalRequestResponse> GetSendMoneyApprovalRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}
