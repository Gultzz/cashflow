using CashFlow.Communication.Requests;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;
using CashFlow.Domain.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
    {
        private readonly IExpensesReadOnlyRepository _repositoryExpenses;
        public GenerateExpenseReportExcelUseCase(IExpensesReadOnlyRepository repositoryExpenses)
        {
            _repositoryExpenses = repositoryExpenses;
        }
        public async Task<byte[]> Execute(RequestInformationReportJson request)
        {
            var expenses = await _repositoryExpenses.FilterByMonth(request.MonthYear);
            if(expenses.Count == 0)
            {
                return [];
            }
            using var workbook = new XLWorkbook();
            
            workbook.Author = "CashFlow";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Calibri";


            var worksheet = workbook.Worksheets.Add($"Relatório {request.MonthYear.ToString("Y")}");
            InsertHeader(worksheet);
            var paymentSymbol = ResourceReportGenerationMessages.PAYMENT_SYMBOL;

            foreach (var (expense, index) in expenses.Select((v, i) => (v, i)))
            {
                var raw = index + 2;
                worksheet.Cell($"A{raw}").Value = expense.Title;
                worksheet.Cell($"B{raw}").Value = expense.Description;
                worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();
                worksheet.Cell($"D{raw}").Value = expense.Date.ToString("F");
                worksheet.Cell($"E{raw}").Value = expense.Amount;
                worksheet.Cell($"E{raw}").Style.NumberFormat.Format = $"-{paymentSymbol} #,##0.00";
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();
            workbook.SaveAs(file);

            return file.ToArray();
            
        }

        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DESCRIPTION;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.AMOUNT;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#99EEAA");
            worksheet.Cells("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        }
    }
}
