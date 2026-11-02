using AutoMapper;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {

        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        public GetAllExpensesUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseGetAllExpensesJson> Execute()
        {
            var loggedUser = await _loggedUser.GetUser();

            var result = await _repository.GetAll(loggedUser);
            var response = new ResponseGetAllExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result),
            };
            return response;
        }
    }
}
