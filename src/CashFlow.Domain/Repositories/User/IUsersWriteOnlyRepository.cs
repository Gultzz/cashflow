namespace CashFlow.Domain.Repositories.User
{
    public interface IUsersWriteOnlyRepository
    {
            Task Add(Entities.User user);

            Task<bool> Delete(Entities.User user);
    }
}
