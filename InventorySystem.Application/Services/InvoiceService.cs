using InventorySystem.Application.DTOs.Invoices;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Accounts;
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
                return Result<InvoiceDto>.Failure($"Delivery: {createInvoiceDto.DeliveryId} is not valid !");

            var salesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(delivery.SalesOrderId, cancellationToken);
            if(salesOrder == null)
                return Result<InvoiceDto>.Failure($"SalesOrder: {delivery.SalesOrderId} is not valid !");

            var salesOrderLines = salesOrder.Lines.ToList();

            var invoiceLines = new List<InvoiceLine>();
            foreach(var invoice_line_Dto in createInvoiceDto.CreateInvoiceLineDtos)
            {
                var delivery_line = delivery.Lines.FirstOrDefault(d => d.ProductId == invoice_line_Dto.ProductId && d.RowNumber == invoice_line_Dto.RowNumber);
                if (delivery_line == null)
                    return Result<InvoiceDto>.Failure($"Product : {invoice_line_Dto.ProductId} is not existed in Delivery !");

                var salesOrderLine = salesOrderLines.FirstOrDefault(l => l.ProductId == delivery_line.ProductId && l.RowNumber == delivery_line.RowNumber);
                if(salesOrderLine == null)
                    return Result<InvoiceDto>.Failure($"Product : {invoice_line_Dto.ProductId} is not existed in SalesOrder !");

                if (invoice_line_Dto.InvoiceQuantity > delivery_line.RemainingInvoicedQty)
                    return Result<InvoiceDto>.Failure($"Invocie {invoice_line_Dto.RowNumber} has Quantity: {invoice_line_Dto.InvoiceQuantity} > [delivery]: {delivery_line.RemainingInvoicedQty}");

                var invoiceLine = new InvoiceLine()
                {
                    DeliveryId = delivery.Id,
                    ProductId = invoice_line_Dto.ProductId,
                    RowNumber = invoice_line_Dto.RowNumber,
                    Quantity = invoice_line_Dto.InvoiceQuantity,
                    // Invoice (Price) must be get from Sales
                    UnitPrice = salesOrderLine.UnitPrice
                };
                invoiceLines.Add(invoiceLine);

                // update invoicedQty in DelieveryLine
                delivery_line.InvoicedQty += invoice_line_Dto.InvoiceQuantity;
            }

            // If no lines => error
            if (!invoiceLines.Any())
                return Result<InvoiceDto>.Failure("Invoice has no valid lines");

            var invoice = new Invoice
            {
                SalesOrderId = delivery.SalesOrderId,
                DeliveryId = delivery.Id,
                InvoiceDate = DateTime.UtcNow,
                Lines = invoiceLines,
                TotalAmount = invoiceLines.Sum(l => l.LineTotal)
            };

            await _unitOfWork.InvoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            var dto = MapToDto(invoice);
            return Result<InvoiceDto>.Success(dto);
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

        public async Task<Result> PostAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var invoice = await _unitOfWork.InvoiceRepository.GetWithLinesAsync(id, cancellationToken);
                if (invoice == null)
                    return Result.Failure($"Invoice {id} is not existed !");

                if (invoice.Status != InvoiceStatus.Draft)
                    return Result.Failure($"Invoice {id} is not DRAFT Status !");

                var invoiceNumber = await _invoiceGenerator.GenerateAsync(cancellationToken);
                invoice.Status = InvoiceStatus.Posted;
                invoice.InvoiceNumber = invoiceNumber;

                var journalEntryLines = new List<JournalEntryLine>();
                journalEntryLines.Add(new JournalEntryLine
                {
                    AccountId = (int)AccountCode.AccountsReceivable,
                    Debit = invoice.TotalAmount,
                    Credit = 0,
                    Description = $"INV:{invoice.InvoiceNumber}"
                });
                foreach (InvoiceLine line in invoice.Lines)
                {
                    journalEntryLines.Add(new JournalEntryLine
                    {
                        AccountId = (int)AccountCode.Revenue,
                        Debit = 0,
                        Credit = line.LineTotal,
                        Description = $"INV:{id}: Product: {line.ProductId} * {line.Quantity}"
                    });
                }

                var journalEntry = new JournalEntry
                {
                    Reference = invoice.InvoiceNumber,
                    InvoiceId = invoice.Id,
                    Lines = journalEntryLines
                };
                await _unitOfWork.JournalEntryRepository.AddAsync(journalEntry);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(ex.Message);
            }
            
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
                    DeliveryId = l.DeliveryId,
                    ProductId = l.ProductId,
                    RowNumber = l.RowNumber,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    LineTotal = l.LineTotal,
                }).ToList(),
            };
        }

        #endregion
    }
}
