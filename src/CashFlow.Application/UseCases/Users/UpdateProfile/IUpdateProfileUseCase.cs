using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.UpdateProfile
{
    public interface IUpdateProfileUseCase
    {
        public Task Execute(RequestUpdateUserJson request);
    }
}
