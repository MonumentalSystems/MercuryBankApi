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
}
