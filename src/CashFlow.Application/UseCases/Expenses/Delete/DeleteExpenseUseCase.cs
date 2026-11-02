using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repositoryWriteOnly;
        private readonly IExpensesReadOnlyRepository _repositoryReadOnly;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repositoryWriteOnly, IExpensesReadOnlyRepository repositoryReadOnly, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
        {
            _repositoryWriteOnly = repositoryWriteOnly;
            _repositoryReadOnly = repositoryReadOnly;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
        }
        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.GetUser();
            var expense = await _repositoryReadOnly.GetById(loggedUser, id);

            if(expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _repositoryWriteOnly.Delete(id);

            await _unitOfWork.Commit();
        }
    }
}
