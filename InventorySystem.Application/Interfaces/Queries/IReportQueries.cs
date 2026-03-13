using InventorySystem.Application.DTOs.Reports;
using InventorySystem.Application.DTOs.TrialBalances;

namespace InventorySystem.Application.Interfaces.Queries
{
    public interface IReportQueries
    {
        Task<IEnumerable<TrialBalanceDto>> GetTrialBalanceAsync(TrialBalanceRequest request, CancellationToken cancellationToken = default);
        Task<IEnumerable<GeneralLedgerDto>> GetGeneralLedgerAsync(GeneralLedgerRequest request, CancellationToken cancellationToken = default);
        Task<IEnumerable<IncomeStatementDto>> GetIncomeStatementAsync(IncomeStatementRequest request, CancellationToken cancellationToken = default);
    }
}
