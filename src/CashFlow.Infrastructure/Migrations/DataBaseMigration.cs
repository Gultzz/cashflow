using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations
{
    public static class DataBaseMigration
    {
        public static async Task MigrateDatabase(IServiceProvider service)
        {
            await using var dbContext = service.GetRequiredService<CashFlowDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
