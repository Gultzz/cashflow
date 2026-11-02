

namespace CashFlow.Domain.Repositories.User
{
    public interface IUsersReadOnlyRepository
    {
        public Task<bool> ExistActiveUserWithEmail(string email);
        public Task<Entities.User?> GetUserByEmail(string email);

    }
}
