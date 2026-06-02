using System.Net;

namespace CashFlow.Exception.ExceptionBase
{
    public class ErrorOnValidationException : CashFlowException
    {
        private List<string> _errors { get; set; }

        public override int StatusCode => (int)HttpStatusCode.BadRequest;

        public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
        {
            _errors = errorMessages;
        }
        public override List<string> GetErrors()
        {
            return _errors;
        }
    }
}
