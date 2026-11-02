using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Security;
using CashFlow.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {

        
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);

            services.AddScoped<IPasswordEncripter, BCrypty>();
            services.AddScoped<ILoggedUser, LoggedUser>();

            AddToken(services, configuration);
            AddRepositories(services);

        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTime = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTime, signingKey!));
        }

        private static void AddRepositories(IServiceCollection services)
        { 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();

            services.AddScoped<IUsersReadOnlyRepository, UsersRepository>();
            services.AddScoped<IUsersWriteOnlyRepository, UsersRepository>();
            services.AddScoped<IUsersUpdateOnlyRepository, UsersRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("Connection");

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }
    }
}
