using InventorySystem.Application.DTOs.Invoices;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Invoice;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceGenerator _invoiceGenerator;

        public InvoiceService(IUnitOfWork unitOfWork, IInvoiceGenerator invoiceGenerator)
        {
            _unitOfWork = unitOfWork;
            _invoiceGenerator = invoiceGenerator;
        }

        #region GETs

        public async Task<Result<List<InvoiceDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var invoices = await _unitOfWork.InvoiceRepository.GetAllWithLinesAsync(cancellationToken);
            var dtos = invoices.Select(MapToDto).ToList();
            return Result<List<InvoiceDto>>.Success(dtos);
        }

        public async Task<Result<InvoiceDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetWithLinesAsync(id, cancellationToken);
            if (invoice == null)
                return Result<InvoiceDto>.Failure($"Invoice {id} is not existed !");

            var dto = MapToDto(invoice);
            return Result<InvoiceDto>.Success(dto);
        }

        #endregion

        #region CRUDs

        public async Task<Result<InvoiceDto>> CreateAsync(CreateInvoiceDto createInvoiceDto, CancellationToken cancellationToken = default)
        {
            var delivery = await _unitOfWork.DeliveryRepository.GetWithLinesAsync(createInvoiceDto.DeliveryId, cancellationToken);
            if (delivery == null || delivery.Status != DeliveryStatus.Posted)
                return Result<InvoiceDto>.Failure("Delivery is not valid !");

            throw new NotImplementedException();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return Result.Failure($"Invoice : {id} is not existed !");

            invoice.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await _unitOfWork.InvoiceRepository.ExistsAsync(i => i.Id == id);
            return Result<bool>.Success(isExist);
        }

        #endregion

        #region Actions

        public Task<Result> PostAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        private static InvoiceDto MapToDto(Invoice entity)
        {
            return new InvoiceDto
            {
                Id = entity.Id,
                SalesOrderId = entity.SalesOrderId,
                DeliveryId = entity.DeliveryId,
                InvoiceDate = entity.InvoiceDate,
                TotalAmount = entity.TotalAmount,
                Status = entity.Status,
                Lines = entity.Lines.Select(l => new InvoiceLineDto
                {
                    Id = l.Id,
                    InvoiceId = l.InvoiceId,
                    DeliveryLineId = l.DeliveryLineId,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    LineTotal = l.LineTotal,
                }).ToList(),
            };
        }

        #endregion
    }
}
