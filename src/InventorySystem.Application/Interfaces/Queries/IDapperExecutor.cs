namespace InventorySystem.Application.Interfaces;

/// <summary>
/// Dapper helper to avoid repeating connection/transaction boilerplate.
/// Implemented in Infrastructure and uses the EF Core connection/transaction when available.
/// </summary>
public interface IDapperExecutor
{
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default);
    Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken cancellationToken = default);
}

