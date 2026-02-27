using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.StockTransfer;

namespace InventorySystem.Application.Services;

public class StockTransferService : IStockTransferService
{
    private readonly IUnitOfWork _unitOfWork;

    public StockTransferService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<StockTransfer>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.StockTransferRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<StockTransfer>>.Success(entities);
    }

    public async Task<Result<StockTransfer>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.StockTransferRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<StockTransfer>.Failure($"StockTransfer with ID {id} not found.");
        }

        return Result<StockTransfer>.Success(entity);
    }
}

