using InventorySystem.Application.DTOs.Reports;
using InventorySystem.Application.DTOs.TrialBalances;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<Result<IEnumerable<TrialBalanceDto>>> GetTrialBalanceAsync(TrialBalanceRequest request, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<GeneralLedgerDto>>> GetGeneralLedgerAsync(GeneralLedgerRequest request, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<IncomeStatementDto>>>GetIncomeStatementAsync(IncomeStatementRequest request, CancellationToken cancellationToken = default);
    }
}
