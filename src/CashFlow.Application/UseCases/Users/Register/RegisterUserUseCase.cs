using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {

        private readonly IMapper _mapper;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IUsersReadOnlyRepository _userReadOnlyRepository;
        private readonly IUsersWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IAccessTokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfwork;

        public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter, IUsersReadOnlyRepository userReadOnlyRepository, IUsersWriteOnlyRepository userWriteOnlyRepository, IAccessTokenGenerator tokenGenerator, IUnitOfWork unitOfwork) { 
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _tokenGenerator = tokenGenerator;
            _unitOfwork = unitOfwork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<User>(request);
            user.Password = _passwordEncripter.Encrypt(request.Password);
            user.Role = Roles.TEAM_MEMBER;
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);

            await _unitOfwork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = _tokenGenerator.Generate(user)
            };
        }

        private async Task Validate(RequestRegisterUserJson request) { 
            var result = new RegisterUserValidator().Validate(request);

            var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if(!!emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREDY_EXISTS));
            }

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
