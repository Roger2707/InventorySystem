using InventorySystem.Application.Interfaces;
using InventorySystem.Infrastructure.Data;
using InventorySystem.Infrastructure.Repositories.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace InventorySystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;
    private IWarehouseRepository? _warehouseRepository;
    private IUserRepository? _userRepository;
    private IRoleRepository? _roleRepository;
    private IPermissionRepository? _permissionRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);
        
        if (_repositories.ContainsKey(type))
        {
            return (IRepository<T>)_repositories[type];
        }

        var repository = new Repository<T>(_context);
        _repositories[type] = repository;
        return repository;
    }

    public IWarehouseRepository WarehouseRepository
    {
        get
        {
            if (_warehouseRepository == null)
            {
                _warehouseRepository = new WarehouseRepository(_context);
            }
            return _warehouseRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
            }
            return _userRepository;
        }
    }

    public IRoleRepository RoleRepository
    {
        get
        {
            if (_roleRepository == null)
            {
                _roleRepository = new RoleRepository(_context);
            }
            return _roleRepository;
        }
    }

    public IPermissionRepository PermissionRepository
    {
        get
        {
            if (_permissionRepository == null)
            {
                _permissionRepository = new PermissionRepository(_context);
            }
            return _permissionRepository;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

