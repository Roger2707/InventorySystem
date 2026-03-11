using InventorySystem.Application.DTOs.Invoices;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Invoice;

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

        public Task<Result<List<InvoiceDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<InvoiceDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CRUDs

        public Task<Result<InvoiceDto>> CreateAsync(CreateInvoiceDto createInvoiceDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
