using MercuryBankApi.Generated;

namespace MercuryBankApi;

/// <summary>
/// High-level client for the Mercury Bank API.
/// </summary>
public interface IMercuryBankClient
{
    /// <summary>Lists all accounts.</summary>
    Task<IReadOnlyList<Account>> GetAccountsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a single account by ID.</summary>
    Task<Account> GetAccountAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>Lists transactions across all accounts for the given date range.</summary>
    Task<IReadOnlyList<Transaction>> GetTransactionsAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default);

    /// <summary>Lists transactions for a specific account.</summary>
    Task<IReadOnlyList<Transaction>> GetAccountTransactionsAsync(Guid accountId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);

    /// <summary>Gets a single transaction by ID.</summary>
    Task<Transaction> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);
}
