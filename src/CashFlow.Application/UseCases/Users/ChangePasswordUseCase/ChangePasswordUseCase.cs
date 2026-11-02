using AutoMapper;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePasswordUseCase
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUsersUpdateOnlyRepository _usersUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfwork;
        private readonly IPasswordEncripter _passwordEncripter;

        public ChangePasswordUseCase(ILoggedUser loggedUser, IUsersUpdateOnlyRepository usersUpdateOnlyRepository, IAccessTokenGenerator tokenGenerator, IUnitOfWork unitOfwork, IPasswordEncripter passwordEncripter)
        {
            _loggedUser = loggedUser;
            _usersUpdateOnlyRepository = usersUpdateOnlyRepository;
            _unitOfwork = unitOfwork;
            _passwordEncripter = passwordEncripter;
        }

        public async Task Execute(RequestChangePasswordJson  request)
        {
            var loggedUser = await _loggedUser.GetUser();

            Validate(request, loggedUser);

            var user = await _usersUpdateOnlyRepository.GetUserById(loggedUser.Id);

            user.Password = _passwordEncripter.Encrypt(request.NewPassword);

            _usersUpdateOnlyRepository.Update(user);

            await _unitOfwork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, User loggedUser)
        {
            var result = new ChangePasswordValidator().Validate(request);

            var passwordMatch = _passwordEncripter.Verify(request.CurrentPassword, loggedUser.Password);

            if(!passwordMatch)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
