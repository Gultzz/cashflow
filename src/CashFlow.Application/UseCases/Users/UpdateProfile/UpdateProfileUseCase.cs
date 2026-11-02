using AutoMapper;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.UpdateProfile
{
    public class UpdateProfileUseCase : IUpdateProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;
        private readonly IUsersWriteOnlyRepository _usersWriteOnlyRepository;
        private readonly IUsersUpdateOnlyRepository _usersUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfwork;

        public UpdateProfileUseCase(ILoggedUser loggedUser, IUsersReadOnlyRepository usersReadOnlyRepository, IUsersWriteOnlyRepository usersWriteOnlyRepository, IUsersUpdateOnlyRepository usersUpdateOnlyRepository, IAccessTokenGenerator tokenGenerator, IUnitOfWork unitOfwork)
        {
            _loggedUser = loggedUser;
            _usersReadOnlyRepository = usersReadOnlyRepository;
            _usersWriteOnlyRepository = usersWriteOnlyRepository;
            _usersUpdateOnlyRepository = usersUpdateOnlyRepository;
            _unitOfwork = unitOfwork;
        }

        public async Task Execute(RequestUpdateUserJson request)
        {

            var loggedUser = await _loggedUser.GetUser();

            await Validate(request, loggedUser.Email);

            var user = await _usersUpdateOnlyRepository.GetUserById(loggedUser.Id);

            user.Email = request.Email;
            user.Name = request.Name;

            _usersUpdateOnlyRepository.Update(user);

            await _unitOfwork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var result = new UpdateProfileValidator().Validate(request);


            if (!currentEmail.Equals(request.Email))
            {
                var emailExist = await _usersReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

                if (!!emailExist)
                {
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREDY_EXISTS));
                }
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
