using InventorySystem.Application.DTOs;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result<WarehouseDto>.Failure($"Warehouse with ID {id} not found.");
        }

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result<IEnumerable<WarehouseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _unitOfWork.WarehouseRepository.GetAllAsync(cancellationToken);
        var warehouseDtos = warehouses.Select(MapToDto);
        return Result<IEnumerable<WarehouseDto>>.Success(warehouseDtos);
    }

    public async Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto createDto, CancellationToken cancellationToken = default)
    {
        // Check if warehouse code already exists
        if (await _unitOfWork.WarehouseRepository.ExistsByCodeAsync(createDto.WarehouseCode, cancellationToken))
        {
            return Result<WarehouseDto>.Failure($"Warehouse with code '{createDto.WarehouseCode}' already exists.");
        }

        var warehouse = MapToEntity(createDto);
        await _unitOfWork.WarehouseRepository.AddAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result<WarehouseDto>> UpdateAsync(int id, UpdateWarehouseDto updateDto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result<WarehouseDto>.Failure($"Warehouse with ID {id} not found.");
        }

        // Check if warehouse code is being changed and if new code already exists
        if (warehouse.WarehouseCode != updateDto.WarehouseCode)
        {
            var existingWarehouse = await _unitOfWork.WarehouseRepository.GetByCodeAsync(updateDto.WarehouseCode, cancellationToken);
            if (existingWarehouse != null && existingWarehouse.Id != id)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with code '{updateDto.WarehouseCode}' already exists.");
            }
        }

        MapToEntity(updateDto, warehouse);
        await _unitOfWork.WarehouseRepository.UpdateAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _unitOfWork.WarehouseRepository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result.Failure($"Warehouse with ID {id} not found.");
        }

        // Soft delete
        warehouse.IsDeleted = true;
        await _unitOfWork.WarehouseRepository.UpdateAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.WarehouseRepository.ExistsAsync(w => w.Id == id, cancellationToken);
        return Result<bool>.Success(exists);
    }

    // Manual mapping methods
    private static WarehouseDto MapToDto(Warehouse warehouse)
    {
        return new WarehouseDto
        {
            Id = warehouse.Id,
            WarehouseCode = warehouse.WarehouseCode,
            WarehouseName = warehouse.WarehouseName,
            Address = warehouse.Address,
            PhoneNumber = warehouse.PhoneNumber,
            ManagerId = warehouse.ManagerId,
            IsActive = warehouse.IsActive,
            Description = warehouse.Description,
            CreatedAt = warehouse.CreatedAt,
            UpdatedAt = warehouse.UpdatedAt
        };
    }

    private static Warehouse MapToEntity(CreateWarehouseDto createDto)
    {
        return new Warehouse
        {
            WarehouseCode = createDto.WarehouseCode,
            WarehouseName = createDto.WarehouseName,
            Address = createDto.Address,
            PhoneNumber = createDto.PhoneNumber,
            ManagerId = createDto.ManagerId,
            IsActive = createDto.IsActive,
            Description = createDto.Description
        };
    }

    private static void MapToEntity(UpdateWarehouseDto updateDto, Warehouse warehouse)
    {
        warehouse.WarehouseCode = updateDto.WarehouseCode;
        warehouse.WarehouseName = updateDto.WarehouseName;
        warehouse.Address = updateDto.Address;
        warehouse.PhoneNumber = updateDto.PhoneNumber;
        warehouse.ManagerId = updateDto.ManagerId;
        warehouse.IsActive = updateDto.IsActive;
        warehouse.Description = updateDto.Description;
    }
}

