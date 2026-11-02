namespace CashFlow.Domain.Repositories.User
{
    public interface IUsersUpdateOnlyRepository
    {
        void Update(Entities.User user);
        Task<Entities.User?> GetUserById(long id);

    }
}
