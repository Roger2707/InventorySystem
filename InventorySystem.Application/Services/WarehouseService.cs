using InventorySystem.Application.DTOs;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IRepository<Warehouse> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(
        IRepository<Warehouse> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result<WarehouseDto>.Failure($"Warehouse with ID {id} not found.");
        }

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result<IEnumerable<WarehouseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _repository.GetAllAsync(cancellationToken);
        var warehouseDtos = warehouses.Select(MapToDto);
        return Result<IEnumerable<WarehouseDto>>.Success(warehouseDtos);
    }

    public async Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto createDto, CancellationToken cancellationToken = default)
    {
        // Check if warehouse code already exists
        var existingWarehouse = await _repository.FirstOrDefaultAsync(
            w => w.WarehouseCode == createDto.WarehouseCode, 
            cancellationToken);

        if (existingWarehouse != null)
        {
            return Result<WarehouseDto>.Failure($"Warehouse with code '{createDto.WarehouseCode}' already exists.");
        }

        var warehouse = MapToEntity(createDto);
        await _repository.AddAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result<WarehouseDto>> UpdateAsync(int id, UpdateWarehouseDto updateDto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result<WarehouseDto>.Failure($"Warehouse with ID {id} not found.");
        }

        // Check if warehouse code is being changed and if new code already exists
        if (warehouse.WarehouseCode != updateDto.WarehouseCode)
        {
            var existingWarehouse = await _repository.FirstOrDefaultAsync(
                w => w.WarehouseCode == updateDto.WarehouseCode && w.Id != id, 
                cancellationToken);

            if (existingWarehouse != null)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with code '{updateDto.WarehouseCode}' already exists.");
            }
        }

        MapToEntity(updateDto, warehouse);
        await _repository.UpdateAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var warehouseDto = MapToDto(warehouse);
        return Result<WarehouseDto>.Success(warehouseDto);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (warehouse == null)
        {
            return Result.Failure($"Warehouse with ID {id} not found.");
        }

        // Soft delete
        warehouse.IsDeleted = true;
        await _repository.UpdateAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(w => w.Id == id, cancellationToken);
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
            Area = warehouse.Area,
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
            Area = createDto.Area,
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
        warehouse.Area = updateDto.Area;
        warehouse.ManagerId = updateDto.ManagerId;
        warehouse.IsActive = updateDto.IsActive;
        warehouse.Description = updateDto.Description;
    }
}

