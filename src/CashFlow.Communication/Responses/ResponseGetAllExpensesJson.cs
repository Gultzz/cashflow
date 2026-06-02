using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Responses
{
    public class ResponseGetAllExpensesJson
    {
        public List<ResponseShortExpenseJson> Expenses { get; set; } = [];
    }

}
