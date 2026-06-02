using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is CashFlowException) {
                HandleProjectException(context);
            } else
            {
                ThrowUnknownError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            var cashflowException = (CashFlowException)context.Exception;
            var err = new ResponseErrorJson(cashflowException.GetErrors());

            context.HttpContext.Response.StatusCode = cashflowException.StatusCode;
            context.Result = new ObjectResult(err);

        }

        private void ThrowUnknownError(ExceptionContext context)
        {
            var err = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);


            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(err);
        }
    }
}
