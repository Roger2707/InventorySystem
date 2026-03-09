using InventorySystem.Application.DTOs.SalesOrder;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.SalesOrder;

namespace InventorySystem.Application.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalesOrderGenerator _salesOrderGenerator;

        public SalesOrderService(IUnitOfWork unitOfWork, ISalesOrderGenerator salesOrderGenerator)
        {
            _unitOfWork = unitOfWork;
            _salesOrderGenerator = salesOrderGenerator;
        }

        #region GET

        public async Task<Result<List<SalesOrderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var salesOrders = await _unitOfWork.SalesOrderRepository.GetAllWithLinesAsync(cancellationToken);
            var dtos = salesOrders.Select(MapToDto).ToList();
            return Result<List<SalesOrderDto>>.Success(dtos);
        }

        public async Task<Result<SalesOrderDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var salesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
            if (salesOrder == null)
                return Result<SalesOrderDto>.Failure($"SalesOrder with ID: {id} is not existed !");

            var dto = MapToDto(salesOrder);
            return Result<SalesOrderDto>.Success(dto);
        }

        #endregion

        #region CRUD

        public async Task<Result<SalesOrderDto>> CreateAsync(CreateSalesOrderDto createSalesOrderDto, CancellationToken cancellationToken = default)
        {
            #region Validations

            var isCustomerExisted = await _unitOfWork.CustomerRepository.ExistsAsync(c => c.Id == createSalesOrderDto.CustomerId, cancellationToken);
            if (!isCustomerExisted)
                return Result<SalesOrderDto>.Failure($"Customer ID {createSalesOrderDto.CustomerId} is not Existed !");

            //var inventory_product = await _unitOfWork.InventoryCostLayerRepository.GetFIFOProductById()

            #endregion

            throw new NotImplementedException();
        }
        public Task<Result<SalesOrderDto>> UpdateAsync(int id, UpdateSalesOrderDto updateSalesOrderDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await _unitOfWork.SalesOrderRepository.ExistsAsync(s => s.Id == id, cancellationToken);
            return Result<bool>.Success(isExist);
        }

        #endregion

        public Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ConfirmAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private static SalesOrderDto MapToDto(SalesOrder entity)
        {
            return new SalesOrderDto
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                OrderDate = entity.OrderDate,
                Status = entity.Status,
                CustomerName = entity.Customer.CustomerName,
                LinesDto = entity.Lines.Select(l => new SalesOrderLineDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.Product.Name,
                    DeliveredQty = l.DeliveredQty,
                    OrderedQty = l.OrderedQty,
                    UnitPrice = l.UnitPrice,
                }).ToList(),
            };
        }

        #endregion
    }
}
