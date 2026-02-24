using System.Data;
using Dapper;
using InventorySystem.Application.Interfaces;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace InventorySystem.Infrastructure.Queries;

public sealed class DapperExecutor : IDapperExecutor
{
    private readonly ApplicationDbContext _db;

    public DapperExecutor(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        var connection = await EnsureOpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, param, cancellationToken);
        var rows = await connection.QueryAsync<T>(command);
        return rows.AsList();
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        var connection = await EnsureOpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, param, cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<T>(command);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        var connection = await EnsureOpenConnectionAsync(cancellationToken);
        var command = CreateCommand(sql, param, cancellationToken);
        return await connection.ExecuteAsync(command);
    }

    private async Task<IDbConnection> EnsureOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = _db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }
        return connection;
    }

    private CommandDefinition CreateCommand(string sql, object? param, CancellationToken cancellationToken)
    {
        var transaction = _db.Database.CurrentTransaction?.GetDbTransaction();
        return new CommandDefinition(sql, parameters: param, transaction: transaction, cancellationToken: cancellationToken);
    }
}

