using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Data;
using Infrastructure;

namespace Inventory.Infrastructure.Queries;

public sealed class DapperExecutor : EfDapperExecutorBase<ApplicationDbContext>, Inventory.Application.Interfaces.IDapperExecutor
{
    public DapperExecutor(ApplicationDbContext db)
        : base(db)
    {
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        var result = await base.QueryAsync<T>(sql, param, cancellationToken: cancellationToken);
        return result.ToList().AsReadOnly();
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        return await base.QueryFirstOrDefaultAsync<T>(sql, param, cancellationToken: cancellationToken);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken cancellationToken = default)
    {
        return await base.ExecuteAsync(sql, param, cancellationToken: cancellationToken);
    }
}




