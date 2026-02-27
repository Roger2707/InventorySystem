using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Services;

public class InventoryCostLayerService : IInventoryCostLayerService
{
    private readonly IUnitOfWork _unitOfWork;

    public InventoryCostLayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<InventoryCostLayer>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.InventoryCostLayerRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<InventoryCostLayer>>.Success(entities);
    }

    public async Task<Result<InventoryCostLayer>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.InventoryCostLayerRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<InventoryCostLayer>.Failure($"InventoryCostLayer with ID {id} not found.");
        }

        return Result<InventoryCostLayer>.Success(entity);
    }
}

