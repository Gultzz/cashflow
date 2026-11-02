using AutoMapper;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.DeleteAccount
{
    public class DeleteAccountUseCase : IDeleteAccountUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;
        private readonly IUsersWriteOnlyRepository _usersWriteOnlyRepository;
        private readonly IUsersUpdateOnlyRepository _usersUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfwork;

        public DeleteAccountUseCase( ILoggedUser loggedUser, IUsersReadOnlyRepository usersReadOnlyRepository, IUsersWriteOnlyRepository usersWriteOnlyRepository, IUsersUpdateOnlyRepository usersUpdateOnlyRepository, IAccessTokenGenerator tokenGenerator, IUnitOfWork unitOfwork)
        {
            _loggedUser = loggedUser;
            _usersReadOnlyRepository = usersReadOnlyRepository;
            _usersWriteOnlyRepository = usersWriteOnlyRepository;
            _usersUpdateOnlyRepository = usersUpdateOnlyRepository;
            _unitOfwork = unitOfwork;
        }
        public async Task Execute()
        {
            var loggedUser = await _loggedUser.GetUser();

            await _usersWriteOnlyRepository.Delete(loggedUser);

            await _unitOfwork.Commit();

        }
    }
}
