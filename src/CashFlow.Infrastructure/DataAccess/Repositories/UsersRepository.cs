using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class UsersRepository : IUsersReadOnlyRepository, IUsersWriteOnlyRepository, IUsersUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public UsersRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email));
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User?> GetUserById(long id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }

        public async Task<bool> Delete(User user)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == user.Id);
            if (result == null)
            {
                return false;
            }
            _dbContext.Users.Remove(result);

            return true;
        }

    }
}
