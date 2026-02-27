using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Services;

public class InventoryLedgerService : IInventoryLedgerService
{
    private readonly IUnitOfWork _unitOfWork;

    public InventoryLedgerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<InventoryLedger>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.InventoryLedgerRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<InventoryLedger>>.Success(entities);
    }

    public async Task<Result<InventoryLedger>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.InventoryLedgerRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<InventoryLedger>.Failure($"InventoryLedger with ID {id} not found.");
        }

        return Result<InventoryLedger>.Success(entity);
    }
}

