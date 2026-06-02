using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    public interface IGenerateExpenseReportPdfUseCase
    {
        public Task<byte[]> Execute(RequestInformationReportJson request);
    }
}
