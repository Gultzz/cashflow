using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        public string Generate(User user);
    }
}
