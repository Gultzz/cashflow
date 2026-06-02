using AutoMapper;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {

        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        public GetAllExpensesUseCase(IExpensesReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseGetAllExpensesJson> Execute()
        {
            var result = await _repository.GetAll();
            var response = new ResponseGetAllExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result),
            };
            return response;
        }
    }
}
