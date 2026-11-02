using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.ChangePasswordUseCase
{
    public interface IChangePasswordUseCase
    {
        public Task Execute(RequestChangePasswordJson request);
    }
}
