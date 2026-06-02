using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions
{
    public static class PaymentTypeExtension
    {
        public static string PaymentTypeToString(this PaymentTypeEnum paymentType)
        {
            return paymentType switch
            {
                PaymentTypeEnum.Cash => ResourceReportGenerationMessages.CASH,
                PaymentTypeEnum.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
                PaymentTypeEnum.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
                PaymentTypeEnum.Transfer => ResourceReportGenerationMessages.ELETRONIC_TRANSFER,
                _ => string.Empty,
            };
        }
    }
}
