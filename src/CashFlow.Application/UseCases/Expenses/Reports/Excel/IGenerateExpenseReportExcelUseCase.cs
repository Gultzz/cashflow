using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    public interface IGenerateExpenseReportExcelUseCase
    {
        public Task<byte[]> Execute(RequestInformationReportJson request);
    }
}
