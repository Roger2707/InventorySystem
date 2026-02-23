using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<Customer?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.CustomerCode == customerCode, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.CustomerCode == customerCode, cancellationToken);
    }

    public async Task<List<string>> GetAllCustomerCodeAsync(CancellationToken cancellationToken)
    {
        var codes = await _dbSet.Select(w => w.CustomerCode).ToListAsync(cancellationToken);
        return codes;
    }
}

