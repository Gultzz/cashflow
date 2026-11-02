using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
    {
        public readonly CashFlowDbContext _dbContext;
        public ExpensesRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
        }

        public async Task<List<Expense>> FilterByMonth(User user,DateTime date)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
            var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
            var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59, millisecond: 999, microsecond:999);
            return await _dbContext.Expenses.AsNoTracking().Where(e => e.Date >= startDate && e.Date <= endDate && e.UserId == user.Id).OrderByDescending(e => e.Date).ThenBy(e => e.Title).ToListAsync();
        }

        public async Task Delete(long id)
        {
            var result = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id);

            _dbContext.Expenses.Remove(result!);
        }

        public void Update(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
        }

        public async Task<List<Expense>> GetAll(User user)
        {
            return await _dbContext.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
        }

        async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
        {
            return await _dbContext.Expenses.Include(expense => expense.Tags).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
        }

        async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
        {
            return await _dbContext.Expenses.Include(expense => expense.Tags).FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
        }
    }
}
