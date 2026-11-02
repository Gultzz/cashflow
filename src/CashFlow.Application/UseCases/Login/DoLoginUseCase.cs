using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {

        private readonly IUsersReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public DoLoginUseCase(IUsersReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator) {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request) {

            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var isCorrectPassword = _passwordEncripter.Verify(request.Password, user.Password);

            if(!isCorrectPassword)
            {
                    throw new InvalidLoginException();
            }

            var token = _accessTokenGenerator.Generate(user);

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = token
            };
        }
    }
}
