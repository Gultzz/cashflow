using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportController : ControllerBase
    {
        [HttpPost("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel([FromServices] IGenerateExpenseReportExcelUseCase useCase, [FromQuery] RequestInformationReportJson request)
        {
            var fileInBytes = await useCase.Execute(request);

            if (fileInBytes.Length > 0) { 
                return File(fileInBytes, MediaTypeNames.Application.Octet, $"report-{request.MonthYear.ToString("Y").Replace(" ", "-")}.xlsx");
            }
            return NoContent();
        }

        [HttpPost("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdf([FromServices] IGenerateExpenseReportPdfUseCase useCase, [FromQuery] RequestInformationReportJson request)
        {
            var fileInBytes = await useCase.Execute(request);

            if (fileInBytes.Length > 0)
            {
                return File(fileInBytes, MediaTypeNames.Application.Pdf, $"report-{request.MonthYear.ToString("Y").Replace(" ", "-")}.pdf");
            }

            return NoContent();
        }
    }
}
